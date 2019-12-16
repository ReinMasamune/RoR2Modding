namespace AlternateArtificer.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class SkirtFix : MonoBehaviour
    {
        public void Awake()
        {
            Single pWscale = 0.9f;
            Single pDscale = 0.9f;
            Single tWscale = 0.9f;
            Single tDscale = 0.9f;
            Single tOffset = -0.005f;
            Single cWscale = 1.1f;
            Single cDscale = 1.1f;

            Single s1z = 0.8f;
            Single s2z = 0.4f;
            Single s3z = 0.4f;

            Transform skirtParent = gameObject.transform.Find("MageArmature").Find("ROOT").Find("base").Find("pelvis");
            skirtParent.localScale = new Vector3( pWscale, 1f, pDscale );

            Transform tl = skirtParent.Find( "thigh.l" );
            Transform tr = skirtParent.Find( "thigh.r" );
            tl.localScale = new Vector3( tWscale, 1f, tDscale );
            tr.localScale = new Vector3( tWscale, 1f, tDscale );
            tl.localPosition += new Vector3( 0f, 0f, tOffset );
            tr.localPosition += new Vector3( 0f, 0f, tOffset );


            Transform cl = tl.Find( "calf.l" );
            Transform cr = tr.Find( "calf.r" );
            cl.localScale = new Vector3( cWscale / (pWscale * tWscale), 1f, cDscale / (pDscale * tDscale) );
            cr.localScale = new Vector3( cWscale / (pWscale * tWscale), 1f, cDscale / (pDscale * tDscale) );


            Vector3 back1Scale = new Vector3( 1f, 1.2f, s1z / pDscale );
            Vector3 back2Scale = new Vector3( 1f, 1.2f, s2z / pDscale );
            Vector3 back3Scale = new Vector3( 1f, 1.2f, s3z / pDscale );
            Vector3 frontScale = new Vector3( 1f, 1f, 1f / pDscale );

            Vector3 back1Offset = new Vector3( 0f, -0.065f, 0f );
            Vector3 back2Offset = new Vector3( 0f, -0.065f, 0f );
            Vector3 back3Offset = new Vector3( 0f, -0.065f, 0f );
            Vector3 frontOffset = new Vector3( 0f, 0f, 0f );

            Transform back1l = skirtParent.Find("ClothA.1.l");
            Transform back1r = skirtParent.Find("ClothA.1.r");
            back1l.localScale = back1Scale;
            back1r.localScale = back1Scale;
            back1l.localPosition += back1Offset;
            back1r.localPosition += back2Offset;

            Transform back2l = skirtParent.Find("ClothB.1.l");
            Transform back2r = skirtParent.Find("ClothB.1.r");
            back2l.localScale = back2Scale;
            back2r.localScale = back2Scale;
            back2l.localPosition += back2Offset;
            back2r.localPosition += back2Offset;

            Transform back3l = skirtParent.Find("ClothC.1.l");
            Transform back3r = skirtParent.Find("ClothC.1.r");
            back3l.localScale = back3Scale;
            back3r.localScale = back3Scale;
            back3l.localPosition += back3Offset;
            back3r.localPosition += back3Offset;

            Transform front = skirtParent.Find("ClothD.1");
            front.localScale = frontScale;
            front.localPosition += frontOffset;


            GameObject skirt = new GameObject("skirt");
            skirt.transform.parent = skirtParent;
            skirt.transform.localScale = Vector3.one;
            skirt.transform.localPosition = new Vector3( 0f, 0f, 0f );
            skirt.transform.localRotation = Quaternion.identity;

            DynamicBone bone = GetComponent<DynamicBone>();
            bone.m_Root = skirt.transform;
            bone.m_Elasticity *= 0.5f;
        }
    }
}
