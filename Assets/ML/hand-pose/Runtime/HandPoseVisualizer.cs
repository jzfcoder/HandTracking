
// used to visualize hand
// first instantiates objects, then changes their transform (to optimize load times)
// a modified script of Yusuf Olokoba (original creator of NatML)

namespace NatSuite.ML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using Vision;

    /// <summary>
    /// Hand skeleton visualizer.
    /// </summary>
    public sealed class HandPoseVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a hand.
        /// </summary>
        /// <param name="image">Image which hand is detected from.</param>
        /// <param name="hand">Hand to render.</param>
        /// 
        public void Render (HandPosePredictor.Hand hand)
        {
            if(first)
            {
                firstRender(hand);
                first = false;
            }
            else
            {
                liveRender(hand);
            }
        }

        public void active(bool isVisible)
        {
            if(!isVisible)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        private void firstRender (HandPosePredictor.Hand hand)
        {
            Debug.Log("First Render Starting");

            // Clear current
            foreach (var t in currentHand)
                GameObject.Destroy(t.gameObject);
            currentHand.Clear();
            
            // Check
            if (hand == default)
                return;

            // Create anchors
            int i = 0;
            foreach (var p in hand)
            {
                var anchor = Instantiate(anchorPrefab, p, Quaternion.identity, transform);
                anchor.gameObject.SetActive(true);
                anchor.gameObject.GetComponent<getPosition>().position = p;
                anchor.gameObject.GetComponent<getPosition>().landmarkNum = i;
                currentHand.Add(anchor);
                i++;
            }
            
            // Create bones
            foreach (var positions in new [] {
                new [] { hand.wrist, hand.thumbCMC, hand.thumbMCP, hand.thumbIP, hand.thumbTip },
                new [] { hand.wrist, hand.indexMCP, hand.indexPIP, hand.indexDIP, hand.indexTip },
                new [] { hand.middleMCP, hand.middlePIP, hand.middleDIP, hand.middleTip },
                new [] { hand.ringMCP, hand.ringPIP, hand.ringDIP, hand.ringTip },
                new [] { hand.wrist, hand.pinkyMCP, hand.pinkyPIP, hand.pinkyDIP, hand.pinkyTip },
                new [] { hand.indexMCP, hand.middleMCP, hand.ringMCP, hand.pinkyMCP }
            })
            {
                var bone = Instantiate(bonePrefab, transform.position, Quaternion.identity, transform);
                bone.gameObject.SetActive(true);
                bone.positionCount = positions.Length;
                bone.SetPositions(positions);
                currentHand.Add(bone.transform);
            }
        }

        private void liveRender(HandPosePredictor.Hand hand)
        {
            for (int i = 21; i < currentHand.Count; i++)
            {
                GameObject.Destroy(currentHand[i].gameObject);
            }
            currentHand.Clear();
            
            if (hand == default)
                return;

            int j = 0;
            foreach (var p in hand)
            {
                GameObject landmark = gameObject.transform.GetChild(j).gameObject;
                landmark.transform.position = p;
                currentHand.Add(landmark.transform);
                j++;
            }

            foreach (var positions in new []
            {
                new [] { hand.wrist, hand.thumbCMC, hand.thumbMCP, hand.thumbIP, hand.thumbTip },
                new [] { hand.wrist, hand.indexMCP, hand.indexPIP, hand.indexDIP, hand.indexTip },
                new [] { hand.middleMCP, hand.middlePIP, hand.middleDIP, hand.middleTip },
                new [] { hand.ringMCP, hand.ringPIP, hand.ringDIP, hand.ringTip },
                new [] { hand.wrist, hand.pinkyMCP, hand.pinkyPIP, hand.pinkyDIP, hand.pinkyTip },
                new [] { hand.indexMCP, hand.middleMCP, hand.ringMCP, hand.pinkyMCP }
            })
            {
                var bone = Instantiate(bonePrefab, transform.position, Quaternion.identity, transform);
                bone.gameObject.SetActive(true);
                bone.positionCount = positions.Length;
                bone.SetPositions(positions);
                currentHand.Add(bone.transform);
            }
        }

        
        #endregion


        #region --Operations--
        [SerializeField] Transform anchorPrefab;
        [SerializeField] LineRenderer bonePrefab;
        List<Transform> currentHand = new List<Transform>();
        bool first = true;
        #endregion
    }
}