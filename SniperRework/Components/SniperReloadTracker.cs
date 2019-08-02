using RoR2;
using UnityEngine;

namespace ReinSniperRework
{
    public class SniperReloadTracker : MonoBehaviour
    {
        public ReinDataLibrary data;

        private bool canShoot = true;
        private bool isReloading = false;
        private bool pastMinTime = false;
        private float reloadTimer;
        private float attackSpeed = 1.0f;
        private float scaledAttackSpeed = 1.0f;
        private float curMinReload;
        private float curMaxReload;
        private int reloadTier;

        Texture2D barTexture;
        Texture2D sliderTexture;
        Rect barPos;
        int sliderY;
        int sliderX1;
        int sliderX2;

        int width;
        int height;

        float frac;

        public void Awake()
        {
            width = Screen.width;
            height = Screen.height;
            GenerateBar();
        }

        public void FixedUpdate()
        {
            if (isReloading)
            {
                reloadTimer += Time.fixedDeltaTime;

                if (reloadTimer > curMinReload & !pastMinTime)
                {
                    reloadTimer -= curMinReload;
                    pastMinTime = true;
                }

                if (reloadTimer > curMaxReload)
                {
                    reloadTimer -= curMaxReload;
                }

                if (pastMinTime)
                {
                    frac = reloadTimer / curMaxReload;
                }
            }
        }

        public void OnGUI()
        {
            if (isReloading && pastMinTime)
            {
                GUI.Label(barPos, barTexture);
                int curSliderX = (int)((sliderX2 - sliderX1) * frac + sliderX1);
                GUI.Label(new Rect(curSliderX, sliderY, width, width), sliderTexture);
            }
        }

        public void Shot()
        {
            canShoot = false;
            reloadTimer = 0.0f;
            pastMinTime = false;
            isReloading = true;
        }

        public bool Reload()
        {
            if (isReloading)
            {
                if (pastMinTime)
                {
                    canShoot = true;
                    isReloading = false;
                    reloadTier = 0;

                    string reloadFeedback = "";

                    if (reloadTimer > data.sr_baseSoftSpotStart * curMaxReload && reloadTimer < data.sr_baseSoftSpotEnd * curMaxReload)
                    {
                        reloadTier = 1;
                    }
                    if (reloadTimer > data.sr_baseSweetSpotStart * curMaxReload && reloadTimer < data.sr_baseSweetSpotEnd * curMaxReload)
                    {
                        reloadTier = 2;
                        reloadFeedback = "Play_item_proc_crit_cooldown";
                    }


                    Util.PlaySound(reloadFeedback, gameObject);
                    //Chat.AddMessage(reloadFeedback);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Something tried to reload while not reloading... good luck");
                return false;
            }
        }

        public bool CanReload()
        {
            return pastMinTime && isReloading;
        }

        public bool FireReady()
        {
            return canShoot;
        }

        public int GetReloadTier()
        {
            return reloadTier;
        }

        public void UpdateAttackSpeed(float AS)
        {
            attackSpeed = AS;

            curMinReload = data.sr_baseMinReloadTime / attackSpeed;

            scaledAttackSpeed = (1f + data.sr_attackSpeedSoft * (1f - 1f / AS));

            curMaxReload = data.sr_baseMaxReloadTime / Mathf.Min(AS, scaledAttackSpeed);
        }

        private void GenerateBar()
        {
            int bar1Start = (int)(data.sr_baseSoftSpotStart * data.sr_totalBarWidth);
            int bar1End = (int)(data.sr_baseSoftSpotEnd * data.sr_totalBarWidth);

            int bar2Start = (int)(data.sr_baseSweetSpotStart * data.sr_totalBarWidth);
            int bar2End = (int)(data.sr_baseSweetSpotEnd * data.sr_totalBarWidth);

            barTexture = new Texture2D(data.sr_totalBarWidth + 2, data.sr_totalBarHeight + 2, TextureFormat.ARGB32, false);

            for (int x = 0; x < barTexture.width; x++)
            {
                for (int y = 0; y < barTexture.height; y++)
                {
                    if (x == 0 || y == 0 || x == barTexture.width - 1 || y == barTexture.height - 1)
                    {
                        barTexture.SetPixel(x, y, data.sr_borderColor);
                    }
                    else
                    {
                        if (x > bar2Start + 2 && x < bar2End)
                        {
                            barTexture.SetPixel(x, y, data.sr_bar2Color);
                        }
                        else
                        {
                            if (x > bar1Start + 2 && x < bar1End)
                            {
                                barTexture.SetPixel(x, y, data.sr_bar1Color);
                            }
                            else
                            {
                                barTexture.SetPixel(x, y, data.sr_baseColor);
                            }
                        }
                    }
                }
            }

            barTexture.Apply();

            sliderX1 = (int)((width - data.sr_totalBarWidth) / 2f + data.sr_barHOffset);
            sliderX2 = (int)((width + data.sr_totalBarWidth) / 2f + data.sr_barHOffset);
            sliderY = (int)((height - data.sr_totalBarHeight) / 2f - data.sr_barVOffset);

            barPos = new Rect(sliderX1 - 1, sliderY - 1, width, width);

            sliderTexture = new Texture2D(data.sr_sliderWidth, data.sr_sliderHeight, TextureFormat.ARGB32, false);

            for (int x = 0; x < sliderTexture.width; x++)
            {
                for (int y = 0; y < sliderTexture.height; y++)
                {
                    sliderTexture.SetPixel(x, y, data.sr_sliderColor);
                }
            }

            sliderX1 -= Mathf.FloorToInt(data.sr_sliderWidth / 2f);
            sliderX2 -= Mathf.FloorToInt(data.sr_sliderWidth / 2f);
            sliderY -= Mathf.FloorToInt((data.sr_sliderHeight - data.sr_totalBarHeight) / 2f);

            sliderTexture.Apply();
        }
    }
}