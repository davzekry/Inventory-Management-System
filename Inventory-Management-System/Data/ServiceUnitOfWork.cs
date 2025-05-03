using Inventory_Management_System.Service;
using Inventory_Management_System.Service.Interfaces;

namespace Inventory_Management_System.Data
{
    public class ServiceUnitOfWork : IServiceUnitOfWork
    {
        private readonly IUnitOfWork unitOfWork;
        //private readonly IConfiguration config;
        private IProductService _ProductService;
        private ITransactionService _TransactionService;
        private IEmailService emailService;

        public ServiceUnitOfWork(IUnitOfWork unitOfWork, IEmailService emailService)//IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            //this.config = config;
        }

        public IProductService ProductService
        {
            get
            {
                if (_ProductService == null)
                    _ProductService = new ProductService(unitOfWork);
                return _ProductService;
            }
        }

        public ITransactionService TransactionService
        {
            get
            {
                if (_TransactionService == null)
                    _TransactionService = new TransactionService(unitOfWork, emailService);
                    //_TransactionService = new TransactionService(unitOfWork, this);
                return _TransactionService;
            }
        }

        //public IEmailService EmailService
        //{
        //    get
        //    {
        //        if (_EmailService == null)
        //            _EmailService = new EmailService(config);
        //        return _EmailService;
        //    }
        //}

        public void save()
        {
            unitOfWork.save();
        }

    }
}
