using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.client.result.Interface
{
    public interface IPatientList
    {
        void SelectAllPatient(bool selectAll);
        void RefreshPatients();
    }
}
