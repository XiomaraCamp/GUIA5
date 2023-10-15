using XJSC.DTOs.CustomerDTOs;
using XJSC.API.Models.DAL;
using XJSC.API.Models.EN;

namespace XJSC.API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void AddCustomerEndpoints(this WebApplication app)
        {
            app.MapPost("/customer/search", async (SearchQueryCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer
                {
                    Name = customerDTO.Name_Like != null ? customerDTO.Name_Like : string.Empty,
                    LastName = customerDTO.LastName_Like != null ? customerDTO.LastName_Like : string.Empty
                };

                var customers = new List<Customer>();
                int countRow = 0;

                if (customerDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de clientes y contar las filas
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);
                    if (customers.Count > 0)
                        countRow = await customerDAL.CountSearch(customer);
                }
                else
                {
                    // Realizar una búsqueda de clientes sin contar las filas
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);
                }

                // Crear un objeto 'SearchResultCustomerDTO' para almacenar los resultados
                var customerResult = new SearchResultCustomerDTO
                {
                    Data = new List<SearchResultCustomerDTO.CustomerDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'CustomerDTO' y agregarlos al resultado
                customers.ForEach(s => {
                    customerResult.Data.Add(new SearchResultCustomerDTO.CustomerDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        LastName = s.LastName,
                        Address = s.Address
                    });
                });

                // Devolver los resultados
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

            app.MapPost("/customer", async (CreateCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer
                {
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                int result = await customerDAL.Create(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            app.MapPut("/customer", async (EditCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer
                {
                    Id = customerDTO.Id,
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
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
