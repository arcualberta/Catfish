using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Solr;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class EntityIndexService : IEntityIndexService
	{
		protected ISolrIndexService<SolrEntry> _solrIndexService;
		private readonly ErrorLog _errorLog;
		public EntityIndexService(ISolrIndexService<SolrEntry> srv, ErrorLog errorLog)
		{
			_solrIndexService = srv;
			_errorLog = errorLog;
		}

		public bool AddUpdateEntity(Entity entity)
		{
			return true;

            try
            {
				bool status = true;
				//Here is a series of ternary expressions that determines the object type.
				//We can expand this to a series of if/else statements if necessary
				SolrEntry.eEntryType type = typeof(ItemTemplate).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.ItemTemplate
					: typeof(Item).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.Item
					: typeof(CollectionTemplate).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.CollectionTemplate
					: typeof(Collection).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.Collection
					: SolrEntry.eEntryType.Other;

				//TODO: implement the perma link for entities.
				string permaLink = null;

				SolrEntry solrEntry = new SolrEntry()
				{
					Id = entity.Id,
					ObjectType = type,
					Permalink = permaLink
				};

				//Setting up the title field of the solr entry. In the case of catfish entities, the title
				//can be represented by the entity name. However, the entity name can contain multiple values
				//and each such value in turn can can contain multiple vaues, each of which represents a value in 
				//a given language. Each such inner "value"s (which are represented in <text ...> type XML
				//elements contain a unique id and we index those IDs along with the values of the <text ...> elements.
				foreach (var txt in entity.Name.Values)
					solrEntry.SetTitle(txt.Id, txt.Value);

				foreach (MetadataSet ms in entity.MetadataSets)
				{
					foreach (BaseField field in ms.Fields)
					{
						if (typeof(TextField).IsAssignableFrom(field.GetType()))
						{
							foreach (MultilingualText val in (field as TextField).Values)
							{
								foreach (Text txt in val.Values)
								{
									solrEntry.AddContent(txt.Id, txt.Value);
								}
							}
						}
						else if ((typeof(OptionsField).IsAssignableFrom(field.GetType())))
						{
							//Going through all options and selecting the options that have been identified by
							//"selected" = "true". For each such selected option, we index the ID of the option
							//along with text labels of the option in all available languages

							foreach (Option opt in (field as OptionsField).Options.Where(op => op.Selected))
								solrEntry.AddContent(opt.Id, opt.OptionText.GetConcatenatedContent(" | "));
						}
					}
				}

				status &= _solrIndexService.AddUpdate(solrEntry);

				return status;
			}
            catch (Exception ex)
            {
				_errorLog.Log(new Error(ex));
				return false;
			}
			
		}
	}
}
