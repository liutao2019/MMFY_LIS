namespace dcl.client.frame
{
    using System.CodeDom.Compiler;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract(ConfigurationName="svc.basic"), GeneratedCode("System.ServiceModel", "3.0.0.0")]
    public interface ICommonBIZ
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
    }
}

