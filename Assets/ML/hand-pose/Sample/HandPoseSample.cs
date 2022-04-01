
// Modified Hand Pose script from Yusuf Olokoba (creater of NatML)  
// Accesses NatML server upon start, and uses it every Update (every frame)
// Converts WebCamTexture default output (texture) to Texture2D before inputting
// keystrokes during debugging are also placed here
// A timer is also used to determine how long a hand isn't detected, after reaching a threshold, the visualization is deleted

namespace NatSuite.Examples
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using NatSuite.ML;
    using NatSuite.ML.Features;
    using NatSuite.ML.Vision;
    using NatSuite.ML.Visualizers;
    using Stopwatch = System.Diagnostics.Stopwatch;

    public sealed class HandPoseSample : MonoBehaviour
    {

        [Header(@"NatML Hub")]
        public string accessKey;

        [Header(@"Prediction")]
        public Texture2D image;
        public HandPoseVisualizer visualizer;

        [Header(@"Real-Time Modifications")]
        public RawImage camInput;
        public GameObject sphere;

        private MLModelData modelData;
        private HandPosePredictor predictor;
        private MLImageFeature input;
        private MLModel model;
        private HandPosePredictor.Hand hand;
        private int curFrames;

        async void Start()
        {
            // Fetch the model data from Hub
            modelData = await MLModelData.FromHub("@natsuite/hand-pose", accessKey);
            // Deserialize the model
            model = modelData.Deserialize();
            // Create the hand pose predictor
            predictor = new HandPosePredictor(model);
            input = new MLImageFeature(image);
        }

        async void Update()
        {
            bool gonzo = true;

            image = TextureToTexture2D(camInput.texture);
            input = new MLImageFeature(image);
            hand = predictor.Predict(input);
            
            if (hand != null)
            {
                curFrames = 0;
                if(gonzo == true)
                {
                    visualizer.active(true);
                    gonzo = false;
                }
                visualizer.Render(hand);
            }
            else
            {
                curFrames++;
                if (curFrames >= 3 * (int) 1.0f / Time.smoothDeltaTime)
                {
                    visualizer.active(false);
                    gonzo = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
                GameObject cube = GameObject.FindGameObjectsWithTag("interactable")[0];
                cube.transform.position = new Vector3 (127f, 71f, 74f);
                cube.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                cube.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                model.Dispose();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject k = Instantiate(sphere, new Vector3(127, 1000, -1000), Quaternion.identity);
                k.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag("sphere");
                foreach (GameObject g in gos)
                {
                    Destroy(g);
                }
            }
            Destroy(image);
        }

        private Texture2D TextureToTexture2D(Texture texture)
        {
            Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
            Graphics.Blit(texture, renderTexture);

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(renderTexture);
            return texture2D;
        }
    }
}