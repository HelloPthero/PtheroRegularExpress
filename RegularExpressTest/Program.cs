using System.Text.RegularExpressions;

namespace RegularExpressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var sql = $@"select modelId from (
select modelId,
       case when stop = 1 then '12' else '' end +
       case when Potential = 1 then '2'  else '' end +
    case when Qualified = 1 then '3'  else '' end +
    case when Candidate = 1 then '4'  else '' end +
    case when Cooperate = 1 then '5'  else '' end +
    case when OnSale = 1 then '6'  else '' end  as lay
from (
select id as modelId,
       case when DemandAnalyseApprovalState = 8 then 1 else 0 end as Stop, --停用
       case when DemandAnalyseApprovalState != 8 and FirstState != 3 and CurrentLayerId != 3
                     and (CurrentLayerId !=5 or DemandAnalyseApprovalState != 6) and (isOnSale is null or isOnSale = 0) then 1 else 0 end as Potential, --潜在
       case when FirstState =3 and DemandAnalyseApprovalState != 8 and CurrentLayerId != 3 and (CurrentLayerId !=5 or DemandAnalyseApprovalState != 6)
                     and isOnSale = 0 then 1 else 0 end as Qualified,                     --合格
       case when CurrentLayerId = 5 and DemandAnalyseApprovalState = 6 and isOnSale = 0 then 1 else 0 end as Candidate,  --候选
       case when CurrentLayerId = 3 and isOnSale = 0 then 1 else 0 end as Cooperate,      -- 合作
       case when DemandAnalyseApprovalState != 8 and isOnSale = 1 then 1 else 0 end as OnSale            --在营
from (
                   select
                         model.id,
                         case when supplyProduct.CurrentLayerId is null then -1 else supplyProduct.CurrentLayerId end as CurrentLayerId,
                         model.FirstState,
                         model.State as DemandAnalyseApprovalState,
                         case when isOnSale is null then 0 else isOnSale end as isonSale
                  from PDM_ItemModel model
                           left join PDM_CompanyItemModelPrice supplyProduct
                                     on model.Id = supplyProduct.ItemModelId
              ) a)b)c where charindex(lay,!value!)>0";

            string pattern = @"('[a-zA-Z0-9]+')";  //单引号内有数据
            string pattern2 = @"('')";

            var liststr = new List<string>();
            var liststremp = new List<string>();
            foreach (Match match in Regex.Matches(sql, pattern)) 
            {
                liststr.Add(match.Value);
                //Console.WriteLine(match.Value);
            }
            foreach (Match match in Regex.Matches(sql, pattern2))
            {
                liststremp.Add(match.Value);
                //Console.WriteLine(match.Value);
            }

            Console.WriteLine("Hello, World!");
        }
    }
}