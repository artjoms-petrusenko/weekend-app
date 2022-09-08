using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weekend_app.Models;

namespace weekend_app.ViewModels
{
    public class MessageViewModel
    {
        private List<DatabaseModel> _dataBaseModelList;
        public List<DatabaseModel> DataBaseModelList => _dataBaseModelList;
        public MessageViewModel(List<DatabaseModel> dataList)
        {
            _dataBaseModelList = dataList;
        }
    }
}
