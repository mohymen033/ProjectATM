using System;
using System.Xml.Serialization ;
using System.Collections;

namespace ConfigureQCash
{
	/// <summary>
	/// Summary description for Field.
	/// </summary>
	
	public class Field 
	{
		private string m_id ;
		private string m_name ;
		private string m_format ;
		private string m_type ;
		private string m_value ;
		private string m_remarks ;
		private string m_size ;
		private string m_sub ;

		[XmlAttribute("name")]
		public string Name
		{
			get{ return m_name ; }
			set{ m_name = value ;}
		}

		[XmlAttribute("id")]
		public string Id
		{
			get{ return m_id ; }
			set{ m_id = value ;}
		}

		[XmlAttribute("format")]
		public string Format
		{
			get{ return m_format ; }
			set{ m_format = value ;}
		}

		[XmlAttribute("type")]
		public string Type
		{
			get{ return m_type ; }
			set{ m_type = value ;}
		}

		[XmlAttribute("value")]
		public string Value
		{
			get{ return m_value ; }
			set{ m_value = value ;}
		}

		[XmlAttribute("remarks")]
		public string Remarks
		{
			get{ return m_remarks ; }
			set{ m_remarks = value ;}
		}

		[XmlAttribute("size")]
		public string Size
		{
			get{ return m_size ; }
			set{ m_size = value ;}
		}
		
		[XmlAttribute("sub")]
		public string Sub
		{
			get{ return m_sub ; }
			set{ m_sub = value ; }
		}
		
//		[XmlArray()]
//		[XmlArrayItem("SubField", typeof(SubField))]
//		public SubFieldCollection subFields;

		
		public Field()
		{			
//			this.subFields = new SubFieldCollection();			
//			
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
