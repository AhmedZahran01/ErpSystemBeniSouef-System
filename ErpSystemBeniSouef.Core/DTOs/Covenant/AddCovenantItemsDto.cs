using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.Covenant
{
    public class AddCovenantItemsDto
    {
        public int CovenantId { get; set; }
        public ObservableCollection<CovenantItemsDto> CovenantItems { get; set; }
    }
    public class CovenantItemsDto: INotifyPropertyChanged
    {
        public int DisplayUIId { get; set; }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        //public int Quantity { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
          
    }
}
