using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanNumber {
    class Program {
        static void Main(string[] args) {
            StreamWriter Writer = new StreamWriter("RomanOutput.txt");
            for (int i = 1; i <= 10000; i++) {
                string Temp;
                Writer.WriteLine($"{i},{Temp = IntegerToRoman(i)},{RomanToInteger(Temp) == i}");
            }
            Writer.Close();
        }

        public static Dictionary<int, string> RomanMap = new Dictionary<int, string>() {
            [1] = "I",[5] = "V",[10] = "X",[50] = "L",[100] = "C",[500] = "D",
            [1000] = "M",[5000] = "v",[10000] = "x",[50000] = "l",[100000] = "c",[500000] = "d"
        };

        public static string IntegerToRoman(int Value) {//數值轉羅馬數字
            if (Value == 0) return string.Empty;//羅馬數字不存在0
            if (RomanMap.ContainsKey(Value)) return RomanMap[Value];//字典內存在項目直接傳回
            KeyValuePair<int, string> UpperBound = (from t in RomanMap where t.Key - Value >= 0 orderby t.Key select t).FirstOrDefault();//上限值
            KeyValuePair<int, string> LowerBound = (from t in RomanMap where Value - t.Key >= 0 orderby t.Key descending select t).FirstOrDefault();//下限值

            if (RomanMap.ContainsKey(UpperBound.Key)) {//如果上限值存在於字典(數值可能超過字典範圍導致Key為0)
                #region 左方減法檢驗項目
                var Del = (from t in RomanMap where t.Key < UpperBound.Key && Math.Log10(t.Key) % 1 == 0 orderby t.Key descending select t).First();//取得減法可擺放符號
                #endregion
                if (Math.Abs(UpperBound.Key - Value) / (double)Del.Key <= 1)//計算是否允許放置減法符號
                    return Del.Value + UpperBound.Value + IntegerToRoman(Math.Abs(Value - UpperBound.Key + Del.Key));//放置後又方補足數值差額
            }
            return LowerBound.Value + IntegerToRoman(Value - LowerBound.Key);//現又數值減去下限值於右方補足差額
        }

        public static int RomanToInteger(string Value) {//羅馬數字還原
            var find = (from t in RomanMap where Value.IndexOf(t.Value) > -1 orderby t.Key descending select t).First();
            string[] Segments = Value.Split(new string[] { find.Value }, 2, StringSplitOptions.None);
            int Result = find.Key;
            if(Segments[0]?.Length > 0) Result -= RomanToInteger(Segments[0]);
            if(Segments[1]?.Length > 0) Result += RomanToInteger(Segments[1]);
            return Result;
        }
    }
}
