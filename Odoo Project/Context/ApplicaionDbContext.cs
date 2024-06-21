using Microsoft.EntityFrameworkCore;
using Odoo_Project.Models;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<CreditNote> CreditNotes { get; set; }
    public DbSet<Payment> Payments { get; set; }
}
