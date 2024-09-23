# PublicFunction.Converter

In this **class**, we have infrastructures that can **centrally control** For Converter **dataSet**

## Use in .netCore
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Converter.IDataSetConverter, PublicFunction.Converter.DataSetConverter>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Converter.IDataSetConverter DataSetConverter;
    		public BeforeLoginService(PublicFunction.Converter.IDataSetConverter dataSetConverter)
    		{
    		    DataSetConverter = dataSetConverter;
    		}
    	}
  ```

  ## Function
    ```C#
        //this function Get DataSet and return Json string
        public string DataSetToJson(DataSet _DataSet);
        //this function Get DataSet and return List<List<Dictionary<string, object>>> like Json
        public List<List<Dictionary<string, object>>> DataSetToList(DataSet _DataSet);
        ////this function Get ref DataSet and return List<List<Dictionary<string, object>>> like Json
        public List<List<Dictionary<string, object>>> DataSetToList(ref DataSet _DataSet);
    ```
