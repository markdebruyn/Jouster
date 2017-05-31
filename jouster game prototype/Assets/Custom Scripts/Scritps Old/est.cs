using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
    public class est : MonoBehaviour
{

    #region Inspector
    [SpineAnimation]
    public string walk = "walk";

    [SpineAnimation]
    public string gungrab = "gungrab";

    [SpineAnimation]
    public string gunkeep = "gunkeep";

    [SpineEvent]
    public string footstepEvent = "footstep";

    public AudioSource footstepAudioSource;
        #endregion
        public SkeletonAnimation skeletonAnimation;

        // Use this for initialization
        void Start() {
            //skeletonAnimation.state.SetAnimation(1, gungrab, true);

            StartCoroutine(GunGrabRoutine());
            StartCoroutine(GunGrabRoutine2());
            skeletonAnimation.state.SetAnimation(0, walk, true);
            
            
            //skeletonAnimation = GetComponent<SkeletonAnimation>();
        }

        IEnumerator GunGrabRoutine2()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                skeletonAnimation.state.SetAnimation(1, gungrab, false);
                

                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                skeletonAnimation.state.SetAnimation(1, gunkeep, false);
            }
        }


        IEnumerator GunGrabRoutine()
        {
            while (true)
                // Play the walk animation on track 0.
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            skeletonAnimation.state.SetAnimation(0, walk, true);

            // Repeatedly play the gungrab and gunkeep animation on track 1.
        }

            // Update is called once per frame
            void Update() {

    }
}
}
