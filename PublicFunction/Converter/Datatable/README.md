# PublicFunction.Converter

In this **class**, we have infrastructures that can **centrally control** For Converter 

## Use in .netCore
- in **Program.cs**  Add This dependence
  ```C#
    builder.Services.AddScoped<PublicFunction.Converter.IDatatableConvertor, PublicFunction.Converter.DatatableConvertor>();
  ```
- Suppose you want to use functions in a class, just write the following code in its constructor function
  ```C#
    public class Test
    	{
    		private readonly PublicFunction.Converter.IDatatableConvertor iDatatableConvertor;
    		public BeforeLoginService(PublicFunction.Converter.IDatatableConvertor DatatableConvertor)
    		{
    		    iDatatableConvertor = DatatableConvertor;
    		}
    	}
  ```

  ## Function
    ```C#
        // Convert DataTable To string Json
        public string DataTableToJson(DataTable _DataTable);
        // Convert string Json To DataTable
        public DataTable JsonToDataTable(string Json);
        // Convert DataTable To List<Dictionary<string, object>>
        public List<Dictionary<string, object>> DataTableToDictionary(DataTable _DataTable);
        // Convert ref DataTable To List<Dictionary<string, object>>
        public List<Dictionary<string, object>> DataTableToDictionary(ref DataTable _DataTable);
    ```
