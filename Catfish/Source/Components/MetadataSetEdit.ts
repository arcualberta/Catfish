import * as ko from "knockout";
import { MetadataSet } from "../ViewModels/Metadata/MetadataSet.js"

ko.components.register('metadataset', {
    viewModel: MetadataSet,
    template: { require: 'text!/Templates/MetadataSetComponent.html' }
})

ko.applyBindings();
