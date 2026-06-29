using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Customer"/> entities.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>Returns a filtered and sorted list of all customers.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Customer>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);

        /// <summary>Returns the total count of customers matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);

        /// <summary>Returns a customer by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The customer's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>Returns a customer matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact customer name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Customer?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>Persists a new customer to the data store.</summary>
        /// <param name="customer">The customer entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Customer customer, CancellationToken cancellationToken);

        /// <summary>Saves changes to an existing customer.</summary>
        /// <param name="customer">The customer entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken);

        /// <summary>Checks whether another customer (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the customer to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
