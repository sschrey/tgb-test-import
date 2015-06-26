using ShippingService.Business.EF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    public class BlogService: BaseFacade
    {

        
        private ShippingServiceData db;
        public BlogService(ShippingServiceData db)
            : base(db)
        {
            this.db = db;
        }

       

        public Blog AddBlog(string name, string url)
        {
            var blog = new Blog { Name = name, Url = url };
            
            Add<Blog>(blog);
            Save();

            return blog;
            
        }

        public List<Blog> GetAllBlogs()
        {
            var query = from b in GetAll<Blog>()
                        orderby b.Name
                        select b;

            return query.ToList();
        }

       
    }
}
