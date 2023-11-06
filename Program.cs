

class Program {
     static public void Main(string[] args) {
        using (var context = new ExamContext()) {
             //Seed.ClearDB(context);
            Seed.SeedData(context); 

            //Example of testing:
            Random rand = new Random();
            var minPrice = 1.5m + rand.Next(0, 5);
            var maxPrice = 28.5m;
            var res = Solution.Q1(context, "m", minPrice, maxPrice)?.ToList();
           
            /*
            switch (args[1])
            {   
                case "Q1": TestSolution.TestQ1(context, Solution.Q1); return;
                case "Q2": TestSolution.TestQ2(context, Solution.Q2); return;
                case "Q3": TestSolution.TestQ3(context, Solution.Q3); return;
                case "Q4": TestSolution.TestQ4(context, Solution.Q4); return;
                case "Q5": TestSolution.TestQ5(context, Solution.Q5); return;
                case "Q6": TestSolution.TestQ6(context, Solution.Q6); return;
                default: throw new ArgumentException();
            } */      
        }
    }
}
