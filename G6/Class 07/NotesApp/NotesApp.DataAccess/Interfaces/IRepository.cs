using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Interfaces
{
	public interface IRepository<T> where T : BaseEntity
	{
		//CRUD - create, read, update, delete
		List<T> GetAll(); //read
		T GetById(int id); //read
		void Add(T entity); //create
		void Update(T entity);//update
		void Delete(T entity);	//delete
	}
}
