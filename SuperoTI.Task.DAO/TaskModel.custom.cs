using SuperoTI.Task.DAO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperoTI.Task.DAO
{
    public partial class TaskModel
    {
        public string StatusDescription
        {
            get
            {
                switch ((TaskStatusEnum)Status)
                {
                    case TaskStatusEnum.Closed:
                        return "Fechado";
                    case TaskStatusEnum.Opened:
                        return "Aberto";
                    case TaskStatusEnum.Deleted:
                        return "Removido";
                }
                return String.Empty;
            }
        }
    }
}
