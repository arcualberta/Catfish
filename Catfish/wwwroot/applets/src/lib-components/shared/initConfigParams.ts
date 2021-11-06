import { ConfigParams } from '../../models';

export function initConfigParams(props: any): ConfigParams {
  const params: ConfigParams = {
    pageId: null,
    blockId: null,
    params: []
  };

  console.log('Props: ', props);

  ////this.pageId = this.$el.parentElement.getAttribute("page-id");
  ////this.blockId = this.$el.parentElement.getAttribute("block-id");
  ////this.appletName = this.$el.parentElement.getAttribute("data-applet-name");

  ////const dataAttributeNames = Array.from(this.$el.parentElement.attributes)
  ////  .filter(att => (att as Attr).name.startsWith("data-"));
  ////dataAttributeNames.forEach(att => {
  ////  const attrib = (att as Attr);
  ////  this.dataAttributes.push({
  ////    name: attrib.name.substring(5),
  ////    value: attrib.value
  ////  });
  ////});
  ////console.log("Parent Attributes: ", this.dataAttributes);

  ////const store = useStore();
  ////store.commit(Mutations.INIT_APPLET, { pageId: this.pageId, blockId: this.blockId, appletName: this.appletName });

  return params;
}
