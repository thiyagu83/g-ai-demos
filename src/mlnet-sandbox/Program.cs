using Microsoft.ML;
using Microsoft.ML.Data;

var mlContext = new MLContext(seed: 1);


// In-memory training data
var trainingData = new[]
{
    new HouseData { Size = 1.1f, Price = 1.2f },
    new HouseData { Size = 1.9f, Price = 2.3f },
    new HouseData { Size = 2.8f, Price = 3.0f },
    new HouseData { Size = 3.4f, Price = 3.7f }
};
// Load data
var dataView = mlContext.Data.LoadFromEnumerable(trainingData);
// Build pipeline
var pipeline = mlContext.Transforms
    .Concatenate("Features", nameof(HouseData.Size))
    .Append(mlContext.Transforms.CopyColumns(
        outputColumnName: "Label",
        inputColumnName: nameof(HouseData.Price)))
    .Append(mlContext.Regression.Trainers.Sdca());
// Train model
var model = pipeline.Fit(dataView);
// Create prediction engine
var predictor = mlContext.Model.CreatePredictionEngine<HouseData, HousePrediction>(model);
// Predict
var prediction = predictor.Predict(new HouseData { Size = 2.5f });
Console.WriteLine($"Predicted price for house size 2.5 = {prediction.Price:0.00}");

// Sample data class
public class HouseData
{
    [LoadColumn(0)]
    public float Size;

    [LoadColumn(1)]
    public float Price;
}
// Prediction class
public class HousePrediction
{
    [ColumnName("Score")]
    public float Price;
}