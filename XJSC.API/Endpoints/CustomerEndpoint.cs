using XJSC.DTOs.CustomerDTOs;
using XJSC.API.Models.DAL;
using XJSC.API.Models.EN;

namespace XJSC.API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void AddCustomerEndpoint(this WebApplication app)
        {
            app.MapPost("/customer/search", async (SearchQueryCustomerDTO cDTO, CustomerDAL cDAL) =>
            {
                var customer = new Customer
                {
                    Name = cDTO.Name_Like != null ? cDTO.Name_Like : string.Empty,
                    LastName = cDTO.LastName_Like != null ? cDTO.LastName_Like : string.Empty
                };

                var customers = new List<Customer>();
                int countRow = 0;

                if (cDTO.SendRowCount == 2)
                {
                    customers = await cDAL.Search(customer, skip: cDTO.Skip, take: cDTO.Take);
                    if (customers.Count > 0)
                        countRow = await cDAL.CountSearch(customer);
                }
                else
                {
                    customers = await cDAL.Search(customer, skip: cDTO.Skip, take: cDTO.Take);
                }


                var customerResult = new SearchResultCustomerDTO
                {
                    Data = new List<SearchResultCustomerDTO.CustomerDTO>(),
                    CountRow = countRow
                };

                customers.ForEach(s => {
                    customerResult.Data.Add(new SearchResultCustomerDTO.CustomerDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        LastName = s.LastName,
                        Address = s.Address
                    });
                });
                return customerResult;
            });

            app.MapGet("/customer/{id}", async (int id, CustomerDAL customerDAL) =>
            {
                var customer = await customerDAL.GetById(id);
                var customerResult = new GetIdResultCustomerDTO
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Address = customer.Address
                };

                if (customerResult.Id > 0)
                    return Results.Ok(customerResult);
                else
                    return Results.NotFound(customerResult);
            });

            app.MapPost("/customer", async (CreateCustomerDTO createDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer
                {
                    Name = createDTO.Name,
                    LastName = createDTO.LastName,
                    Address = createDTO.Address
                };

                int result = await customerDAL.Create(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            app.MapPut("/customer", async (EditCustomerDTO EditDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer
                {
                    Id = EditDTO.Id,
                    Name = EditDTO.Name,
                    LastName = EditDTO.LastName,
                    Address = EditDTO.Address
                };

                int result = await customerDAL.Edit(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            app.MapDelete("/customer/{id}", async (int id, CustomerDAL customerDAL) =>
            {
                int result = await customerDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
