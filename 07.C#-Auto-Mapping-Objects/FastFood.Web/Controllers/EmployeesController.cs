﻿namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;

    using Data;
    using ViewModels.Employees;
    using FastFood.Models;
    using AutoMapper.QueryableExtensions;
    using System.Linq;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            var positions = this.context.Positions
                    .ProjectTo<RegisterEmployeeViewModel>(mapper.ConfigurationProvider)
                    .ToList();

            return this.View(positions);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = this.mapper.Map<Employee>(model);
            var position = this.context.Positions.FirstOrDefault(p => p.Name == model.PositionName);
            employee.PositionId = position.Id;
            this.context.Employees.Add(employee);

            this.context.SaveChanges();

            return RedirectToAction("All", "Employees");
        }

        public IActionResult All()
        {
            var employees = this.context.Employees
                    .ProjectTo<EmployeesAllViewModel>(mapper.ConfigurationProvider)
                    .ToList();

            return this.View(employees);
        }
    }
}
