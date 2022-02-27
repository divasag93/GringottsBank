using System.Collections.Generic;

namespace GringottsBank.Core
{
    public static class Failure
    {
        public static void BadRequest(List<DataContracts.Error> errors)
        {
            throw new BadRequestException(errors);
        }

        public static void BadRequest(string code, string message)
        {
            throw new BadRequestException(new DataContracts.Error { Code = code, Message = message });
        }
    }
}
