using System;
using System.ComponentModel;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos
{
    public class DueInvoiceDetailsDto : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private int _displayId;
        public int DisplayId
        {
            get => _displayId;
            set
            {
                if (_displayId != value)
                {
                    _displayId = value;
                    OnPropertyChanged(nameof(DisplayId));
                }
            }
        }

        private DateTime _invoiceDate;
        public DateTime InvoiceDate
        {
            get => _invoiceDate;
            set
            {
                if (_invoiceDate != value)
                {
                    _invoiceDate = value;
                    OnPropertyChanged(nameof(InvoiceDate));
                }
            }
        }
         
        private string _supplierName;
        public string SupplierName
        {
            get => _supplierName;
            set
            {
                if (_supplierName != value)
                {
                    _supplierName = value;
                    OnPropertyChanged(nameof(SupplierName));
                }
            }
        }

        private int _supplierId;
        public int SupplierId
        {
            get => _supplierId;
            set
            {
                if (_supplierId != value)
                {
                    _supplierId = value;
                    OnPropertyChanged(nameof(SupplierId));
                }
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                    OnPropertyChanged(nameof(TotalAmountPlusDueAmount)); // مهم
                }
            }
        }

        
        private decimal? _dueAmount;
        public decimal? DueAmount
        {
            get => _dueAmount;
            set
            {
                if (_dueAmount != value)
                {
                    _dueAmount = value;
                    OnPropertyChanged(nameof(DueAmount));
                    OnPropertyChanged(nameof(TotalAmountPlusDueAmount)); // مهم
                }
            }
        }

        private decimal? _totalAmountPlus_dueAmount;
        public decimal? TotalAmountPlusDueAmount
        {
            get 
            {
                if (DueAmount.HasValue)
                    return ((DueAmount * TotalAmount) / 100) + TotalAmount;
                return null;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
