using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContractStatusCodeData
    {
        public static List<StatusCodeModel> ContractStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000001",Language.contract_1st_installment_sts,"#F43927"), // thanh toán đợt 1 contract_1st_installment_sts //  1st Installment 
                new StatusCodeModel("100000003",Language.contract_being_payment_sts,"#F43927"), // contract_being_payment_sts //Being Payment
                new StatusCodeModel("4",Language.contract_canceled_sts,"#8bce3d"), // đã hủy  contract_canceled_sts //Canceled
                new StatusCodeModel("100001",Language.contract_complete_sts,"#FF8F4F"), // hoàn thành// contract_complete_sts //Complete
                new StatusCodeModel("100000004",Language.contract_complete_payment_sts,"#FF8F4F"), // contract_complete_payment_sts //Complete Payment
                new StatusCodeModel("100000007",Language.contract_converted_sts,"#FF8F4F"),// contract_converted_sts //Converted
                new StatusCodeModel("100000005",Language.contract_handover_sts,"#FF8F4F"),// contract_handover_sts //Handover
                new StatusCodeModel("3",Language.contract_in_progress_sts,"#FF8F4F"),// contract_in_progress_sts //In Progress
                new StatusCodeModel("100003",Language.contract_invoiced_sts,"#FF8F4F"),// contract_invoiced_sts //Invoiced
                new StatusCodeModel("1",Language.contract_open_sts,"#FF8F4F"),// contract_open_sts //Open
                new StatusCodeModel("100000000",Language.contract_option_sts,"#ffc43d"), // contract_option_sts //Option
                new StatusCodeModel("100002",Language.contract_partial_sts,"#FF8F4F"),// contract_partial_sts //Partial
                new StatusCodeModel("2",Language.contract_pending_sts,"#FF8F4F"),// contract_pending_sts //Pending
                new StatusCodeModel("100000002",Language.contract_signed_contract_sts,"#FF8F4F"), // đã ký hợp đồng // contract_signed_contract_sts //Signed Contract
                new StatusCodeModel("100000006",Language.contract_terminated_sts,"#c4c4c4"), // đã thanh lý // contract_terminated_sts //Terminated
                new StatusCodeModel("0","","#bfbfbf"),
            };
        }

        public static StatusCodeModel GetContractStatusCodeById(string id)
        {
            return ContractStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
