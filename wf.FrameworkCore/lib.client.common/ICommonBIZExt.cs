namespace dcl.client.frame
{
    using System.ServiceModel;
    using System.Data;
    using System.CodeDom.Compiler;

    [ServiceContract(ConfigurationName = "svc.basic"), GeneratedCode("System.ServiceModel", "3.0.0.0")]
    public interface ICommonBIZExt
	{
        [OperationContract]
        DataSet doDel(DataSet ds);
        [OperationContract]
        DataSet doNew(DataSet ds);
        [OperationContract]
        DataSet doOther(DataSet ds);
        [OperationContract]
        DataSet doSearch(DataSet ds);
        [OperationContract]
        DataSet doUpdate(DataSet ds);
        [OperationContract]
        DataSet doView(DataSet ds);
        [OperationContract]
        DataSet Search(string where);
        [OperationContract]
        int Update(string set, string where);
        [OperationContract]
        int Insert(string columns, string values);
	}
}
