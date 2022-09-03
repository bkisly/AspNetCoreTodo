using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Services;

namespace AspNetCoreTodo.Test
{
    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var writeContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(writeContext);
                var testUser = new IdentityUser { Id = "test-600", UserName = "test@example.com" };
                await service.AddItemAsync(new TodoItem { Title = "Testing?" }, testUser);
            }

            using var readContext = new ApplicationDbContext(options);

            int itemsInDatabase = await readContext.Items.CountAsync();
            Assert.Equal(1, itemsInDatabase);

            TodoItem item = await readContext.Items.FirstAsync();
            Assert.Equal("Testing?", item.Title);
            Assert.False(item.IsDone);

            if(item.DueAt.HasValue)
            {
                TimeSpan offset = DateTimeOffset.Now.AddDays(3) - item.DueAt.Value;
                Assert.True(offset < TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public async Task ThrowAnExceptionWhenMarkingDoneNonExistingItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_MarkDoneExceptionCheck").Options;
            var testUser = new IdentityUser { Id = "test-500", UserName = "test@example.com" };


            using (var writeContext = new ApplicationDbContext(options))
            {
                var writeService = new TodoItemService(writeContext);
                await writeService.AddItemAsync(new TodoItem { Title = "Test item" }, testUser);
            }

            using var readContext = new ApplicationDbContext(options);
            var readService = new TodoItemService(readContext);
            var item = await readContext.Items.FirstAsync();

            await readService.MarkDoneAsync(item.Id, testUser);
            Assert.True(item.IsDone);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await readService.MarkDoneAsync(new Guid(), testUser));
        }

        [Fact]
        public async Task GetItemsOwnedOnlyByTheUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetIncompleteItems").Options;
            var testUser1 = new IdentityUser { Id = "test-500", UserName = "test1@example.com" };
            var testUser2 = new IdentityUser { Id = "test-600", UserName = "test2@example.com" };

            using (var writeContext = new ApplicationDbContext(options))
            {
                var writeService = new TodoItemService(writeContext);
                await writeService.AddItemAsync(new TodoItem { Title = "Item 1" }, testUser1);
                await writeService.AddItemAsync(new TodoItem { Title = "Item 2" }, testUser1);
                await writeService.AddItemAsync(new TodoItem { Title = "Item 3" }, testUser2);
                await writeService.AddItemAsync(new TodoItem { Title = "Item 4" }, testUser2);
            }

            using var readContext = new ApplicationDbContext(options);
            var readService = new TodoItemService(readContext);
            var firstUserItems = await readService.GetIncompleteItemsAsync(testUser1);
            var secondUserItems = await readService.GetIncompleteItemsAsync(testUser2);


            Assert.Equal(2, firstUserItems.Length);
            Assert.Equal(2, secondUserItems.Length);

            Assert.Empty(firstUserItems.Where(item => item.Title == "Item 3" || item.Title == "Item 4"));
            Assert.Empty(secondUserItems.Where(item => item.Title == "Item 1" || item.Title == "Item 2"));
        }
    }
}
