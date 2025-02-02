using Bogus;
using Calderilla.Domain;

namespace Calderilla.Test.Utils
{
    public class FakeOperationGenerator
    {
        public static List<Operation> GetFakeOperations(int count)
        {
            var faker = new Faker<Operation>()
                .RuleFor(o => o.Id, f => f.Random.Guid())
                .RuleFor(o => o.MonthOperationNumber, f => f.Random.Int(1, 12))
                .RuleFor(o => o.OperationDate, f => f.Date.Past())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                .RuleFor(o => o.ValueDate, f => f.Date.Past())
                .RuleFor(o => o.Amount, f => f.Finance.Amount())
                .RuleFor(o => o.Balance, f => f.Finance.Amount())
                .RuleFor(o => o.Payer, f => f.Person.FullName)
                .RuleFor(o => o.Ignore, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.Notes, f => f.Lorem.Paragraph())
                .RuleFor(o => o.Reviewed, f => f.Random.Bool());

            return faker.Generate(count);
        }
    }
}
