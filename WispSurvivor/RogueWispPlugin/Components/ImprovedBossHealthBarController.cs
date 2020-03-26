#if BOSSHPBAR
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class ImprovedBossHealthBarController : MonoBehaviour
        {
            public HUD hud;
            public GameObject container;
            public GameObject background;

            public HGTextMeshProUGUI healthLabel;
            public HGTextMeshProUGUI bossNameLabel;
            public HGTextMeshProUGUI bossSubtitleLabel;

            public List<BossHealthBarSegment> barPanels;

            private Boolean listeningForClientDamageNotified;

            private BossGroup currentBossGroup
            {
                get => this._currentBossGroup;
                set
                {
                    if( this._currentBossGroup != value )
                    {
                        this.bossHealthComps.Clear();
                        this._currentBossGroup = value;

                        foreach( BossHealthBarSegment panel in this.barPanels )
                        {
                            panel.ResetMaxViewed();
                        }

                    }
                    if( value != null )
                    {
                        foreach( CharacterMaster master in this.combatSquadMembersField.GetValue( value.combatSquad ) as List<CharacterMaster> )
                        {
                            CharacterBody body = master.GetBody();
                            if( body )
                            {
                                HealthComponent hc = body.healthComponent;
                                if( hc && !this.bossHealthComps.Contains( hc ) )
                                {
                                    this.bossHealthComps.Add( hc );
                                }
                            }
                        }
                    }
                }
            }
            private BossGroup _currentBossGroup;

            private readonly List<HealthComponent> bossHealthComps = new List<HealthComponent>();
            private FieldInfo combatSquadMembersField;

            private Run.TimeStamp nextAllowedSourceUpdateTime = Run.TimeStamp.negativeInfinity;

            private readonly Dictionary<BarLayer, List<BossHealthBarSegment>> organizedPanels = new Dictionary<BarLayer, List<BossHealthBarSegment>>();


            private static readonly StringBuilder sharedStringBuilder = new StringBuilder();

            public enum BarLayer
            {
                Background = -1,
                Main = 0,
                Barrier = 1
            }
            public class BossHealthBarSegment
            {
                public String name { get; private set; }
                public Boolean includeInMaximum { get; private set; }
                public Boolean includeInCurrent { get; private set; }
                public Single cachedMaxViewed { get; private set; }
                public Single cachedCurrent { get; private set; }
                public Single cachedFrac { get; private set; }
                public Single cachedDelayFrac { get; private set; }
                public BarLayer layer { get; private set; }

                public Func<HealthComponent, Single> getCurrent { get; private set; }
                public Func<HealthComponent, Single> getMax { get; private set; }


                private Single vel = 0f;

                private readonly Slider barFill;
                private readonly Slider delayFill;
                private readonly RectTransform barMaster;

                public struct SegmentInfo
                {
                    public String name;
                    public BarLayer layer;
                    public Boolean includeInMaximum;
                    public Boolean includeInCurrent;
                    public Func<HealthComponent,Single> getCurrent;
                    public Func<HealthComponent,Single> getMax;
                    public Color barFillColor;
                    public Color barDelayColor;
                    public Sprite barFillTex;
                    public Sprite barDelayTex;
                    public Image.Type barFillType;
                    public Image.Type barDelayType;
                    public Transform parent;
                }

                public BossHealthBarSegment( SegmentInfo info )
                {
                    if( !template )
                    {
                        Main.LogF( "Template for BossHealthBarSegment was never created." );
                        return;
                    }

                    this.name = info.name;
                    this.layer = info.layer;
                    this.includeInMaximum = info.includeInMaximum;
                    this.includeInCurrent = info.includeInCurrent;
                    this.getCurrent = info.getCurrent;
                    this.getMax = info.getMax;

                    GameObject obj = Instantiate<GameObject>( template, info.parent );
                    this.barMaster = obj.transform as RectTransform;
                    this.barMaster.gameObject.name = this.name + "BarMaster";

                    this.barFill = this.barMaster.Find( "MainSlider" ).GetComponent<Slider>();
                    this.delayFill = this.barMaster.Find( "DelaySlider" ).GetComponent<Slider>();

                    Image mainImage = this.barFill.fillRect.GetComponent<Image>();
                    Image delayImage = this.delayFill.fillRect.GetComponent<Image>();

                    mainImage.color = info.barFillColor;
                    if( info.barFillTex != null ) mainImage.sprite = info.barFillTex;
                    mainImage.type = info.barFillType;

                    delayImage.color = info.barDelayColor;
                    if( info.barDelayTex != null ) delayImage.sprite = info.barDelayTex;
                    delayImage.type = info.barDelayType;
                }


                public void UpdateValues( Single current, Single max )
                {
                    this.cachedCurrent = current;
                    this.cachedMaxViewed = Mathf.Max( this.cachedMaxViewed, max );
                    this.cachedFrac = this.cachedMaxViewed == 0f ? 0f : (this.cachedCurrent / this.cachedMaxViewed);
                    this.cachedDelayFrac = Mathf.Clamp( Mathf.SmoothDamp( this.cachedDelayFrac, this.cachedFrac, ref this.vel, 0.1f, Single.PositiveInfinity, Time.deltaTime ), this.cachedFrac, 1f );
                    this.barFill.value = this.cachedFrac;
                    this.delayFill.value = this.cachedDelayFrac;
                }

                public Single UpdateGlobals( Single startPos, Single globalFrac, Single scaler )
                {
                    this.barMaster.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Left, startPos * scaler, scaler * globalFrac );
                    return (this.cachedFrac * globalFrac) + startPos;
                }

                public void ResetMaxViewed() => this.cachedMaxViewed = 0f;

                private static GameObject template;
                public static void BuildTemplate( GameObject uiPrefab, Transform parent )
                {
                    Transform baseBarMaster = Instantiate<GameObject>(uiPrefab, parent).transform;
                    baseBarMaster.gameObject.name = "templateBar";

                    Transform baseBarDelay = Instantiate<GameObject>(uiPrefab, baseBarMaster).transform;
                    baseBarDelay.gameObject.name = "DelaySlider";
                    Slider baseBarDelaySlider = baseBarDelay.gameObject.AddComponent<Slider>();

                    Transform baseBarMain = Instantiate<GameObject>(uiPrefab, baseBarMaster).transform;
                    baseBarMain.gameObject.name = "MainSlider";
                    Slider baseBarMainSlider = baseBarMain.gameObject.AddComponent<Slider>();



                    RectTransform baseBarMainTrans = Instantiate<GameObject>( uiPrefab, baseBarMain).transform as RectTransform;
                    baseBarMainTrans.gameObject.name = "SliderFill";

                    RectTransform baseBarDelayTrans = Instantiate<GameObject>( uiPrefab, baseBarDelay).transform as RectTransform;
                    baseBarMainTrans.gameObject.name = "SliderFill";


                    Destroy( baseBarMaster.GetComponent<Image>() );
                    Destroy( baseBarMain.GetComponent<Image>() );
                    Destroy( baseBarDelay.GetComponent<Image>() );


                    baseBarMainSlider.direction = Slider.Direction.LeftToRight;
                    baseBarDelaySlider.direction = Slider.Direction.LeftToRight;
                    baseBarMainSlider.fillRect = baseBarMainTrans;
                    baseBarDelaySlider.fillRect = baseBarDelayTrans;
                    baseBarMainSlider.handleRect = null;
                    baseBarDelaySlider.handleRect = null;
                    baseBarMainSlider.image = null;
                    baseBarDelaySlider.image = null;
                    baseBarMainSlider.interactable = false;
                    baseBarDelaySlider.interactable = false;
                    baseBarMainSlider.maxValue = 1f;
                    baseBarDelaySlider.maxValue = 1f;
                    baseBarMainSlider.minValue = 0f;
                    baseBarDelaySlider.minValue = 0f;
                    baseBarMainSlider.navigation = new Navigation
                    {
                        mode = Navigation.Mode.None
                    };
                    baseBarDelaySlider.navigation = new Navigation
                    {
                        mode = Navigation.Mode.None
                    };
                    baseBarMainSlider.targetGraphic = null;
                    baseBarDelaySlider.targetGraphic = null;
                    baseBarMainSlider.transition = Selectable.Transition.None;
                    baseBarDelaySlider.transition = Selectable.Transition.None;
                    baseBarMainSlider.wholeNumbers = false;
                    baseBarDelaySlider.wholeNumbers = false;

                    template = baseBarMaster.gameObject;
                }

                public static void DeleteTemplate() => Destroy( template );
            }

            private void Awake()
            {
                this.combatSquadMembersField = typeof( CombatSquad ).GetField( "membersList", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public );

                Transform container = base.transform.Find( "BossHealthBarContainer" );
                Transform backgroundPanel = container.Find( "BackgroundPanel" );
                Transform nameLabel = container.Find( "BossNameLabel" );
                Transform subNameLabel = nameLabel.Find( "BossSubtitleLabel" );

                this.container = container.gameObject;
                this.background = backgroundPanel.gameObject;
                this.bossNameLabel = nameLabel.GetComponent<HGTextMeshProUGUI>();
                this.bossSubtitleLabel = subNameLabel.GetComponent<HGTextMeshProUGUI>();

                this.hud = base.transform.root.GetComponent<HUD>();
                HealthBarStyle style = this.hud.GetComponentInChildren<HealthBar>().style;
                Transform basePanel = backgroundPanel.Find( "FillPanel" );
                Transform baseDelayPanel = backgroundPanel.Find( "DelayFillPanel" );

                BossHealthBarSegment.BuildTemplate( basePanel.gameObject, backgroundPanel );




                /*
                var healthDelayPanel = Instantiate<GameObject>(basePanel.gameObject, basePanel.parent );
                healthDelayPanel.name = "HealthDelayPanel";
                var healthDelayImg = healthDelayPanel.GetComponent<Image>();
                healthDelayImg.color = SqLerpColor( style.trailingBarStyle.baseColor, Color.white, 0.75f );
                healthDelayImg.sprite = Instantiate<Sprite>( style.trailingBarStyle.sprite );

                var healthPanel = Instantiate<GameObject>(basePanel.gameObject, basePanel.parent );
                healthPanel.name = "HealthFillPanel";
                var healthPanelImg = healthPanel.GetComponent<Image>();
                healthPanelImg.color = SqLerpColor( style.trailingBarStyle.baseColor, Color.white, 0.5f );
                healthPanelImg.sprite = Instantiate<Sprite>( style.trailingBarStyle.sprite );


                var shieldDelayPanel = Instantiate<GameObject>(basePanel.gameObject, basePanel.parent );
                shieldDelayPanel.name = "ShieldDelayPanel";
                var shieldDelayImg = shieldDelayPanel.GetComponent<Image>();
                shieldDelayImg.color = SqLerpColor( style.shieldBarStyle.baseColor, Color.white, 0.5f );
                shieldDelayImg.sprite = Instantiate<Sprite>( style.shieldBarStyle.sprite );

                var shieldPanel = Instantiate<GameObject>(basePanel.gameObject, basePanel.parent );
                shieldPanel.name = "ShieldFillPanel";
                var shieldPanelImg = shieldPanel.GetComponent<Image>();
                shieldPanelImg.color = style.shieldBarStyle.baseColor;
                shieldPanelImg.sprite = Instantiate<Sprite>( style.shieldBarStyle.sprite );


                var barrierDelayPanel = Instantiate<GameObject>(basePanel.gameObject, baseDelayPanel.parent );
                barrierDelayPanel.name = "BarrierDelayPanel";
                var barrierDelayImg = barrierDelayPanel.GetComponent<Image>();
                barrierDelayImg.color = SqLerpColor( style.barrierBarStyle.baseColor, Color.black, 0.5f ); 
                barrierDelayImg.sprite = Instantiate<Sprite>( style.barrierBarStyle.sprite );

                var barrierPanel = Instantiate<GameObject>(basePanel.gameObject, basePanel.parent );
                barrierPanel.name = "BarrierFillPanel";
                var barrierPanelImg = barrierPanel.GetComponent<Image>();
                barrierPanelImg.color = style.barrierBarStyle.baseColor;
                barrierPanelImg.sprite = Instantiate<Sprite>( style.barrierBarStyle.sprite );
                */




                if( this.barPanels == null ) this.barPanels = new List<BossHealthBarSegment>();
                this.barPanels.Add( new BossHealthBarSegment( new BossHealthBarSegment.SegmentInfo
                {
                    name = "Health",
                    layer = BarLayer.Main,
                    includeInCurrent = true,
                    includeInMaximum = true,
                    parent = backgroundPanel,
                    getCurrent = ( hc ) => hc.health,
                    getMax = ( hc ) => hc.fullHealth,
                    barFillColor = new Color( 0.80f, 0.01f, 0.01f, 1f ),
                    barDelayColor = new Color( 1f, 0.73f, 0.73f, 1f ),
                    barFillType = Image.Type.Simple,
                    barDelayType = Image.Type.Simple,
                } ) );

                this.barPanels.Add( new BossHealthBarSegment( new BossHealthBarSegment.SegmentInfo
                {
                    name = "Shield",
                    layer = BarLayer.Main,
                    includeInCurrent = true,
                    includeInMaximum = false,
                    parent = backgroundPanel,
                    getCurrent = ( hc ) => hc.shield,
                    getMax = ( hc ) => hc.fullShield,
                    barFillColor = style.shieldBarStyle.baseColor,
                    barDelayColor = SqLerpColor( style.shieldBarStyle.baseColor, Color.white, 0.5f ),
                    barFillType = Image.Type.Simple,
                    barDelayType = Image.Type.Simple,
                } ) );

                this.barPanels.Add( new BossHealthBarSegment( new BossHealthBarSegment.SegmentInfo
                {
                    name = "Barrier",
                    layer = BarLayer.Barrier,
                    includeInCurrent = true,
                    includeInMaximum = false,
                    parent = backgroundPanel,
                    getCurrent = ( hc ) => hc.barrier,
                    getMax = ( hc ) => hc.fullBarrier,
                    barFillColor = new Color( 1f, 1f, 1f, 1f ),
                    barDelayColor = new Color( 1f, 1f, 0.5f, 1f ),
                    barFillType = Image.Type.Sliced,
                    barDelayType = Image.Type.Simple,
                    barFillTex = style.barrierBarStyle.sprite
                } ) );


                Transform oldHealthText = backgroundPanel.Find( "HealthText" );
                GameObject healthText = Instantiate<GameObject>( oldHealthText.gameObject, oldHealthText.parent );
                Destroy( oldHealthText.gameObject );
                this.healthLabel = healthText.GetComponent<HGTextMeshProUGUI>();
                Destroy( basePanel.gameObject );
                Destroy( baseDelayPanel.gameObject );
                Destroy( base.GetComponent<HUDBossHealthBarController>() );

                BossHealthBarSegment.DeleteTemplate();
            }

            private void Start()
            {
                foreach( BossHealthBarSegment panel in this.barPanels )
                {
                    if( !this.organizedPanels.ContainsKey( panel.layer ) || this.organizedPanels[panel.layer] == null )
                    {
                        this.organizedPanels[panel.layer] = new List<BossHealthBarSegment>();
                    }

                    this.organizedPanels[panel.layer].Add( panel );
                }
            }

            private void FixedUpdate()
            {
                List<BossGroup> bossGroupInstances = InstanceTracker.GetInstancesList<BossGroup>();
                if( bossGroupInstances.Count != 0 )
                {
                    Int32 counter = 0;
                    foreach( BossGroup group in bossGroupInstances )
                    {
                        if( group.shouldDisplayHealthBarOnHud )
                        {
                            if( counter == 0 ) this.currentBossGroup = group;
                            counter++;
                        }
                    }
                    this.SetListeningForClientDamageNotified( counter > 1 );
                } else
                {
                    this.currentBossGroup = null;
                }
            }

            private void LateUpdate()
            {
                Boolean shouldDisplay = this.currentBossGroup != null && this.currentBossGroup.totalObservedHealth > 0f;
                this.container.SetActive( shouldDisplay );
                if( shouldDisplay )
                {
                    Single totalCurrent = 0f;
                    Single totalMax = 0f;
                    Single widthScaler = ((RectTransform)this.background.transform).rect.width;
                    foreach( BossHealthBarSegment panel in this.barPanels )
                    {
                        Single panelCurrent = 0f;
                        Single panelMax = 0f;
                        foreach( HealthComponent hc in this.bossHealthComps )
                        {
                            if( hc == null ) continue;
                            panelCurrent += panel.getCurrent( hc );
                            panelMax += panel.getMax( hc );
                        }
                        panel.UpdateValues( panelCurrent, panelMax );
                    }

                    foreach( KeyValuePair<BarLayer, List<BossHealthBarSegment>> kv in this.organizedPanels )
                    {
                        Single layerMax = 0f;
                        foreach( BossHealthBarSegment panel in kv.Value )
                        {
                            layerMax += panel.cachedMaxViewed;
                        }

                        Single globalStart = 0f;
                        foreach( BossHealthBarSegment panel in kv.Value )
                        {
                            if( panel.includeInCurrent ) totalCurrent += panel.cachedCurrent;
                            if( panel.includeInMaximum ) totalMax += panel.cachedMaxViewed;
                            globalStart = panel.UpdateGlobals( globalStart, panel.cachedMaxViewed / layerMax, widthScaler );
                        }
                    }

                    sharedStringBuilder.Clear().AppendInt( Mathf.FloorToInt( totalCurrent ) ).Append( "/" ).AppendInt( Mathf.FloorToInt( totalMax ) );
                    this.healthLabel.SetText( sharedStringBuilder );
                    this.bossNameLabel.SetText( this.currentBossGroup.bestObservedName );
                    this.bossSubtitleLabel.SetText( this.currentBossGroup.bestObservedSubtitle );
                }
            }

            private void SetListeningForClientDamageNotified( Boolean newValue )
            {
                if( newValue == this.listeningForClientDamageNotified ) return;

                this.listeningForClientDamageNotified = newValue;

                if( newValue ) GlobalEventManager.onClientDamageNotified += this.OnClientDamageNotified;
                else GlobalEventManager.onClientDamageNotified -= this.OnClientDamageNotified;
            }

            private void OnClientDamageNotified( DamageDealtMessage message )
            {
                if( !this.nextAllowedSourceUpdateTime.hasPassed || !message.victim ) return;
                CharacterBody victimBody = message.victim.GetComponent<CharacterBody>();
                if( !victimBody || !(victimBody.isBoss && message.attacker == this.hud.targetBodyObject) ) return;

                BossGroup group = BossGroup.FindBossGroup( victimBody );
                if( group && group.shouldDisplayHealthBarOnHud )
                {
                    this.currentBossGroup = group;
                    this.nextAllowedSourceUpdateTime = Run.TimeStamp.now + 1f;
                }
            }

            private static Color SqLerpColor( Color color1, Color color2, Single t )
            {
                Single r1 = Mathf.Pow( color1.r, 2 );
                Single g1 = Mathf.Pow( color1.g, 2 );
                Single b1 = Mathf.Pow( color1.b, 2 );
                Single a1 = Mathf.Pow( color1.a, 2 );

                Single r2 = Mathf.Pow( color1.r, 2 );
                Single g2 = Mathf.Pow( color1.g, 2 );
                Single b2 = Mathf.Pow( color1.b, 2 );
                Single a2 = Mathf.Pow( color1.a, 2 );

                return new Color
                {
                    r = Mathf.Sqrt( Mathf.Lerp( r1, r2, t ) ),
                    g = Mathf.Sqrt( Mathf.Lerp( g1, g2, t ) ),
                    b = Mathf.Sqrt( Mathf.Lerp( b1, b2, t ) ),
                    a = Mathf.Sqrt( Mathf.Lerp( a1, a2, t ) ),
                };
            }
        }
    }

}
#endif