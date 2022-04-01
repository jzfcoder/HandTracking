# Hand Pose
Hand pose detection from MediaPipe. This predictor implements the [hand pose model](https://drive.google.com/file/d/1yiPfkhb4hSbXJZaSq9vDmhz24XVZmxpL/preview), but not the palm detector. It only supports detecting a single hand in the image.

## Detecting Hand Pose in an Image
First, create the predictor:
```csharp
// Fetch the model data from NatML Hub
var modelData = await MLModelData.FromHub("@natsuite/hand-pose");
// Deserialize the model
var model = modelData.Deserialize();
// Create the predictor
var predictor = new HandPosePredictor(model);
```


Then detect the hand pose in an image:
```csharp
Texture2D image = ...; // Can also be a `WebCamTexture` or pixel buffer
// Detect hand pose in an image
HandPosePredictor.Hand hand = predictor.Predict(image);
```

## Requirements
- Unity 2019.2+
- NatML 1.0+

## Quick Tips
- See the [NatML documentation](https://docs.natsuite.io/natml)
- Join the [NatSuite community on Discord](https://discord.gg/y5vwgXkz2f)
- Check out the [NatSuite blog](https://blog.natsuite.io/)
- Contact us at [hi@natsuite.io](mailto:hi@natsuite.io)

Thank you very much!