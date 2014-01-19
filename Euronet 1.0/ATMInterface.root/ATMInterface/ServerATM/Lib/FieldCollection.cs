using System;
using System.Xml.Serialization;
namespace ConfigureQCash
{
	/// <summary>
	/// Summary description for FieldCollection.
	/// </summary>
	
	[XmlRoot("Fields")]
	public class FieldCollection : System.Collections.CollectionBase
	{
		
		public FieldCollection()
		{
			new Field();
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// make a collection of field and index each fields
		/// </summary>
		[XmlArrayItem]
		public Field this[int index]
		{
			get
			{
				return (Field)this.List[index];
			}
			set
			{
				this.List[index] = value ;
			}
		}


		/// <summary>
		/// index using the id value
		/// </summary>
		[XmlArrayItem]
		public Field this[string id]
		{
			get
			{
				foreach(Field field in this.List)
					if(field.Id == id )
						return field;
				return null;
			}
			set
			{
				
			}
		}	

		/// <summary>
		/// Method that will Add a Field to the FieldCollection
		/// </summary>
		/// <param name="Field">The Field to add to the FieldCollection</param>
		public void Add(Field field)
		{
			// Add the specified Field to the internal IList
			this.List.Add(field);
		}


		/// <summary>
		/// Method that will insert the specified Field to the FieldCollection at 
		/// the specified index
		/// </summary>
		/// <param name="index">The index at which to Field should be inserted</param>
		/// <param name="Field">The Field to insert into the FieldCollection</param>
		public void Insert(int index, Field field)
		{
			// Insert the specified Field to the internal IList
			this.List.Insert(index, field);
		}


		/// <summary>
		/// Method that will remove the specified Field from the FieldCollection
		/// </summary>
		/// <param name="Field">The Field to be removed from the FieldCollection</param>
		public void Remove(Field field)
		{
			this.List.Remove(field);
		}


		/// <summary>
		/// Method that will return the index of the specified Field in the FieldCollection
		/// </summary>
		/// <param name="Field">The Field whose index is being requested</param>
		/// <returns>The index of the specified Field in the FieldCollection</returns>
		public int IndexOf(Field field)
		{
			// Obtain the index of the Field from the interal IList
			return this.List.IndexOf(field);
		}


		/// <summary>
		/// Method that will determine if the specified Field exists in the FieldCollection
		/// </summary>
		/// <param name="Field">The Field to be located</param>
		/// <returns>A boolean value indicated whether or not the FieldCollection contains the specified Field</returns>
		public bool Contains(Field field)
		{
			// Check to see if the Field exists in the internal IList
			return this.List.Contains(field);
		}
	}
}
