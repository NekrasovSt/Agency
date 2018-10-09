using System;
using Agency.Web.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace Agency.Web
{
  public class ModelBuilder
  {
    public static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
    {
      var builder = new ODataConventionModelBuilder(serviceProvider);

      builder.EntitySet<Announcement>(nameof(Announcement));
      builder.EntitySet<RealEstateObject>(nameof(RealEstateObject));

      return builder.GetEdmModel();
    }
  }
}
