using Domain.Models;
using FakeItEasy;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;
using Infrastructure.Services;

namespace UnitTest;

[TestFixture]
public class JournalServiceTests
{
    private readonly IJournalDataService _fakeDataService;
    private readonly JournalService _journalService;

    public JournalServiceTests()
    {
        _fakeDataService = A.Fake<IJournalDataService>();
        _journalService = new JournalService(_fakeDataService);
    }

    [Test]
    public async Task GetRange_ShouldCallDataService_WhenValidInput()
    {
        // Arrange
        var skip = 0;
        var take = 10;
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;
        var search = "test";
        var cancellationToken = CancellationToken.None;

        var expectedResponse = new JournalListResponse(default, default, default);
        A.CallTo(() => _fakeDataService.Select(skip, take, startDate, endDate, search, cancellationToken))
            .Returns(expectedResponse);

        // Act
        var result = await _journalService.GetRange(skip, take, startDate, endDate, search, cancellationToken);

        // Assert
        Assert.AreEqual(expectedResponse, result);
        A.CallTo(() => _fakeDataService.Select(skip, take, startDate, endDate, search, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void GetRange_ShouldThrowSecureException_WhenSkipIsNegative()
    {
        // Arrange
        var skip = -1;
        var take = 10;
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;
        var search = "test";
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        Assert.ThrowsAsync<SecureException>(async () =>
            await _journalService.GetRange(skip, take, startDate, endDate, search, cancellationToken));
    }

    [Test]
    public void GetRange_ShouldThrowSecureException_WhenTakeIsZeroOrNegative()
    {
        // Arrange
        var skip = 0;
        var take = 0;
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;
        var search = "test";
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        Assert.ThrowsAsync<SecureException>(async () =>
            await _journalService.GetRange(skip, take, startDate, endDate, search, cancellationToken));
    }

    [Test]
    public void GetRange_ShouldThrowSecureException_WhenStartDateIsGreaterThanEndDate()
    {
        // Arrange
        var skip = 0;
        var take = 10;
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(-1);
        var search = "test";
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        Assert.ThrowsAsync<SecureException>(async () =>
            await _journalService.GetRange(skip, take, startDate, endDate, search, cancellationToken));
    }

    [Test]
    public async Task GetSingle_ShouldCallDataService_WhenValidId()
    {
        // Arrange
        var id = 1;
        var cancellationToken = CancellationToken.None;
        var expectedResponse = new JournalResponse(default, default, default, default);

        A.CallTo(() => _fakeDataService.Get(id, cancellationToken)).Returns(expectedResponse);

        // Act
        var result = await _journalService.GetSingle(id, cancellationToken);

        // Assert
        Assert.AreEqual(expectedResponse, result);
        A.CallTo(() => _fakeDataService.Get(id, cancellationToken)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void GetSingle_ShouldThrowSecureException_WhenIdIsInvalid()
    {
        // Arrange
        var id = 0;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        Assert.ThrowsAsync<SecureException>(async () =>
            await _journalService.GetSingle(id, cancellationToken));
    }
}