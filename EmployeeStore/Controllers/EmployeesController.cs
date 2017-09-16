using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeesDataAccess;

namespace EmployeeStore.Controllers
{
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Employee> LoadSoemthing()
        {
            using (EmplyeeDBEntities entities = new EmplyeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadSoemthing(int id)
        {
            using (EmplyeeDBEntities entities = new EmplyeeDBEntities())
            {
                var entity =  entities.Employees.FirstOrDefault(e => e.ID == id);

                return entity != null ?
                    Request.CreateResponse(HttpStatusCode.OK, entity) :
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with id {id} is not found");
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmplyeeDBEntities entities = new EmplyeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri , employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmplyeeDBEntities entities = new EmplyeeDBEntities())
                {

                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity != null)
                    {
                        entities.Employees.Remove(entities.Employees.FirstOrDefault(e => e.ID == id));
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"Employee with id {id} is not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            try
            {
                using (EmplyeeDBEntities entities = new EmplyeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Salary = employee.Salary;
                        entity.Gender = employee.Gender;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"Employee with id {id} is not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
