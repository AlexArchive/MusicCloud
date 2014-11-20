using System.Reflection;
using Xunit;

namespace MusicCloud.AcceptanceTests
{
    public class UseDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            Bootstrap.InstallDatabase();
            base.Before(methodUnderTest);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            base.After(methodUnderTest);
            Bootstrap.UninstallDatabase();
        }
    }
}