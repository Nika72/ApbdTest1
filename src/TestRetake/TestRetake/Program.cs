using System.Collections.Immutable;
using System.Data.SqlClient;
using TestRetake.Entities;
using TestRetake.Repositories;
using TestRetake.Services;
using TestRetake.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<IDepartmentRepository>(provider => new DepartmentRepository(connectionString));
builder.Services.AddSingleton<IEmployeeRepository>(provider => new EmployeeRepository(connectionString));

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/api/departments", async (IDepartmentService departmentService) =>
{
    var departments = await departmentService.GetAllAsync();
    return Results.Ok(departments);
})
.WithName("GetAllDepartments")
.WithOpenApi();

app.MapGet("/api/departments/{id:int}", async (int id, IDepartmentService departmentService) =>
{
    var department = await departmentService.GetByIdAsync(id);
    return department != null ? Results.Ok(department) : Results.NotFound();
})
.WithName("GetDepartmentById")
.WithOpenApi();

app.MapPost("/api/employees", async (AddEmployeeDTO dto, IEmployeeService employeeService, IDepartmentService departmentService) =>
{
    var employee = new Employee
    {
        EmpName = dto.EmpName,
        JobName = dto.JobName,
        ManagerID = dto.ManagerID,
        HireDate = DateTime.UtcNow,
        Salary = dto.Salary,
        Commission = dto.Commission,
        DepID = dto.DepID
    };

    try
    {
        // Check if manager exists
        if (employee.ManagerID.HasValue)
        {
            var manager = await employeeService.GetByIdAsync(employee.ManagerID.Value);
            if (manager == null)
            {
                return Results.BadRequest("Manager not found.");
            }
        }

        var department = await departmentService.GetByIdAsync(employee.DepID);
        if (department == null)
        {
            return Results.BadRequest("Department not found.");
        }

        var employeeId = await employeeService.AddAsync(employee);
        return Results.Created($"/api/employees/{employeeId}", employeeId);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("CreateEmployee")
.WithOpenApi();

app.MapPost("/api/departments", async (AddDepartmentDTO dto, IDepartmentService departmentService) =>
{
    var department = new Department
    {
        DepName = dto.DepName,
        DepLocation = dto.DepLocation
    };

    var departmentId = await departmentService.AddAsync(department);
    return Results.Created($"/api/departments/{departmentId}", departmentId);
})
.WithName("CreateDepartment")
.WithOpenApi();

app.MapPut("/api/employees", async (UpdateEmployeeDTO dto, IEmployeeService employeeService, IDepartmentService departmentService) =>
{
    var employee = new Employee
    {
        EmpID = dto.EmpID,
        EmpName = dto.EmpName,
        JobName = dto.JobName,
        ManagerID = dto.ManagerID,
        Salary = dto.Salary,
        Commission = dto.Commission,
        DepID = dto.DepID
    };

    try
    {
        // Check if manager exists
        if (employee.ManagerID.HasValue)
        {
            var manager = await employeeService.GetByIdAsync(employee.ManagerID.Value);
            if (manager == null)
            {
                return Results.BadRequest("Manager not found.");
            }
        }

        var department = await departmentService.GetByIdAsync(employee.DepID);
        if (department == null)
        {
            return Results.BadRequest("Department not found.");
        }

        var result = await employeeService.UpdateAsync(employee);
        return result ? Results.Ok() : Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("UpdateEmployee")
.WithOpenApi();

app.MapDelete("/api/employees/{id:int}", async (int id, IEmployeeService employeeService) =>
{
    var result = await employeeService.DeleteAsync(id);
    return result ? Results.Ok() : Results.NotFound();
})
.WithName("DeleteEmployee")
.WithOpenApi();

app.Run();
