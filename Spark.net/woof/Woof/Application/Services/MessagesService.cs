using Microsoft.EntityFrameworkCore;
using Woof.Application.Database;
using Woof.Application.Models;

namespace Woof.Application.Services;

public class MessagesService
{

    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public MessagesService(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }


    public async Task Destroy(Message message)
    {
        var context = _factory.CreateDbContext();
        context.Messages.Remove(message);
        context.SaveChanges();
    }



    //public async Task<List<Message>> GetAll()
    //{
    //    using var context = _factory.CreateDbContext();
    //    return await context.Messages.Include(message  => message.User).OrderByDescending(m=>m.CreatedAt).ToListAsync();
    //}



    public async Task<List<Message>> GetAll()
    {
        var context = _factory.CreateDbContext();
        return await context.Messages.ToListAsync();
    }

    public async Task<Message> Update(Message message)
    {
        using var context = _factory.CreateDbContext();
        context.Update(message);
        context.SaveChanges();
        return message;
    }

    public async Task<Message?> Get(int id)
    {
       using var context = _factory.CreateDbContext();
        return await context.Messages
            .Where(message => message.Id == id)
            .Include(message => message.User)
            .FirstOrDefaultAsync();
    }

    public async Task <Message> Store(Message message)
    {
        using var context = _factory.CreateDbContext();
        context.Messages.Add(message);
        context.SaveChanges();
        return message;
    }
}