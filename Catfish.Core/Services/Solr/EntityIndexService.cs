using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Solr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class EntityIndexService : IEntityIndexService
	{
		protected ISolrIndexService<SolrItemModel> _solrIndexService;
		public EntityIndexService(ISolrIndexService<SolrItemModel> srv)
		{
			_solrIndexService = srv;
		}

		public bool AddUpdateEntity(Entity entity)
		{
			bool status = true;
			SolrEntry.eEntryType type = typeof(ItemTemplate).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.ItemTemplate
				: typeof(Item).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.Item
				: typeof(CollectionTemplate).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.CollectionTemplate
				: typeof(Collection).IsAssignableFrom(entity.GetType()) ? SolrEntry.eEntryType.Collection
				: SolrEntry.eEntryType.Other;

			SolrEntry entry = new SolrEntry()
			{
				Id = entity.Id,
				ObjectType = type,
				//Title = string.IsNullOrWhiteSpace(entity.Title) ? null : doc.Title,
				//Excerpt = string.IsNullOrWhiteSpace(doc.Excerpt) ? null : doc.Excerpt,
				//Permalink = string.IsNullOrWhiteSpace(doc.Permalink) ? null : doc.Permalink,
			};
			//entry.EntityGuid.Add(entity.Id);
			//entry.EntityType.Add(entity.ModelType);

			//foreach (MetadataSet ms in entity.MetadataSets)
			//{
			//	foreach (BaseField field in ms.Fields)
			//	{
			//		if (typeof(TextField).IsAssignableFrom(field.GetType()))
			//		{
			//			foreach (MultilingualText val in (field as TextField).Values)
			//			{
			//				foreach (Text txt in val.Values)
			//				{
			//					solrText.MetadataSetGuid.Add(ms.Id);

			//					solrText.FieldGuid.Add(field.Id);
			//					//solrText.ValueGuid.Add();
			//					//solrText.TextGuid.Add();
			//					solrText.Lang.Add(txt.Language);
			//					solrText.Content.Add(txt.Value);
			//				}
			//			}
			//		}
			//	}
			//}

			//status &= _solrIndexService.AddUpdate(solrText);

			return status;
		}

		//public bool AddUpdateEntity(Entity entity)
		//{
		//	bool status = true;

		//	SolrItemModel solrText = new SolrItemModel();
		//	solrText.EntityGuid.Add(entity.Id);
		//	solrText.EntityType.Add(entity.ModelType);

		//	foreach (MetadataSet ms in entity.MetadataSets)
		//	{
		//		foreach(BaseField field in ms.Fields)
		//		{
		//			if(typeof(TextField).IsAssignableFrom(field.GetType()))
		//			{
		//				foreach(MultilingualText val in (field as TextField).Values)
		//				{
		//					foreach (Text txt in val.Values)
		//					{
		//						solrText.MetadataSetGuid.Add(ms.Id);

		//						solrText.FieldGuid.Add(field.Id);
		//						//solrText.ValueGuid.Add();
		//						//solrText.TextGuid.Add();
		//						solrText.Lang.Add(txt.Language);
		//						solrText.Content.Add(txt.Value);
		//					}
		//				}
		//			}
		//		}
		//	}

		//	status &= _solrIndexService.AddUpdate(solrText);

		//	return status;
		//}

	}
}
