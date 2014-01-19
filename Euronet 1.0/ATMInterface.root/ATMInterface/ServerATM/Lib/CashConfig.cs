using System;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigureQCash
{
	/// <summary>
	/// Represents the CashConfig.config XML file as a .NET class
	/// </summary>
	[XmlRoot()]	
	public class CashConfig
	{
		/// <summary>
		/// A constant denoting the CashConfig.config file name
		/// </summary>
		[XmlIgnore()]
		public const string CashConfigFileName = "CashConfig.config";
		/// <summary>
		/// The Fields defined in the CashConfig.config file
		/// </summary>
		[XmlArray()]
		[XmlArrayItem("Field", typeof(Field))]
		public FieldCollection Fields;

		public CashConfig()
		{		
			this.Fields = new FieldCollection();						
		}
		/// <summary>
		/// Saves the CashConfig instance to the specified path
		/// </summary>
		/// <param name="configFilePath">The path to which the PageConfig will be saved</param>
		public void Save(string configFilePath)
		{
			// Create an XmlSerializer to save the Page.config file
			XmlSerializer xs = new XmlSerializer(typeof(CashConfig));

			// Create an XmlTextWriter for saving the serialized XML to a file
			XmlTextWriter writer = new XmlTextWriter(configFilePath, System.Text.Encoding.UTF8);

			// Set the formatting mode for the writer
			writer.Formatting = Formatting.Indented;

			// Serialize the CashConfig into the writer
			xs.Serialize(writer, this);

			// Flush the writer
			writer.Flush();

			// Close the writer
			writer.Close();

		}

		/// <summary>
		/// Deserializes the specified configuration file into a new instance 
		/// of the FieldConfig class
		/// </summary>
		/// <param name="configPath">The path to the configuration file</param>
		/// <returns>The configuration file as an instance of the FieldConfig class</returns>
		public static CashConfig Load(string configFilePath)
		{
			// Load the CashConfig.confg file into an XmlTextReader
			XmlTextReader reader = new XmlTextReader(configFilePath);

			// Create an XmlSerializer to load the CashConfig file
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(CashConfig));

				// Deserialize the CashConfig.config file into a CashConfig object
				CashConfig cc = (CashConfig)xs.Deserialize(reader);

				// Close the XmlTextReader
				reader.Close();

				// Return the deserialized instance
				return cc;				
			}
			catch(Exception e)
			{
				System.Console.WriteLine(e.GetType().ToString());
			}
			return null;
		}

		/// <summary>
		/// according to the id specified it finds the Field Element
		/// </summary>
		public Field FindFieldByID(string id)
		{
			Field foundField = null ;
			foreach(Field field in Fields)
			{
				// Iterate over the Fields ArrayList
				if(field.Id.Equals(id))
				{

					foundField = field ;
					break;

				}
			}

			return foundField;

		}

	}
}
