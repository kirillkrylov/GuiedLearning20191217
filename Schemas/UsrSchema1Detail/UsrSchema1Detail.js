define("UsrSchema1Detail", ["ConfigurationGrid", "ConfigurationGridGenerator",
    "ConfigurationGridUtilities"], function() {
	return {
		entitySchemaName: "UsrTrainingParticipants",
		attributes: {
            // Determines whether the editing is enabled.
            "IsEditable": {
                // Data type — logic.
                dataValueType: Terrasoft.DataValueType.BOOLEAN,
                // Attribute type — virtual column of the view model.
                type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
                // Set value.
                value: true
            }
        },
         mixins: {
            ConfigurationGridUtilities: "Terrasoft.ConfigurationGridUtilities"
        },
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		diff: /**SCHEMA_DIFF*/[
 			{
                // Operation type — merging.
                "operation": "merge",
                // Name of the schema element, with which the action is performed.
                "name": "DataGrid",
                // Object, whose properties will be joined with the schema element properties.
                "values": {
                    // Class name
                    "className": "Terrasoft.ConfigurationGrid",
                    // View generator must generate only part of view.
                    "generator": "ConfigurationGridGenerator.generatePartial",
                    // Binding the edit elements configuration obtaining event
                    // of the active page to handler method.
                    "generateControlsConfig": {"bindTo": "generateActiveRowControlsConfig"},
                    // Binding the active record changing event to handler method.
                    "changeRow": {"bindTo": "changeRow"},
                    // Binding the record selection cancellation event to handler method.
                    "unSelectRow": {"bindTo": "unSelectRow"},
                    // Binding of the list click event to handler method.
                    "onGridClick": {"bindTo": "onGridClick"},
                    // Actions performed with active record.
                    "activeRowActions": [
                        // [Save] action setup.
                        {
                            // Class name of the control element, with which the action is connected.
                            "className": "Terrasoft.Button",
                            // Display style — transparent button.
                            "style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
                            // Tag.
                            "tag": "save",
                            // Marker value.
                            "markerValue": "save",
                            // Binding button image.
                            "imageConfig": {"bindTo": "Resources.Images.SaveIcon"}
                        },
                        // [Cancel] action setup.
                        {
                            "className": "Terrasoft.Button",
                            "style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
                            "tag": "cancel",
                            "markerValue": "cancel",
                            "imageConfig": {"bindTo": "Resources.Images.CancelIcon"}
                        },
                        // [Delete] action setup.
                        {
                            "className": "Terrasoft.Button",
                            "style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
                            "tag": "remove",
                            "markerValue": "remove",
                            "imageConfig": {"bindTo": "Resources.Images.RemoveIcon"}
                        }
                    ],
                    // Binding to method that initializes subscription to events
                    // of clicking buttons in the active row.
                    "initActiveRowKeyMap": {"bindTo": "initActiveRowKeyMap"},
                    // Binding the active record action completion event to handler method.
                    "activeRowAction": {"bindTo": "onActiveRowAction"},
                    // Identifies whether multiple records can be selected.
                    "multiSelect": {"bindTo": "MultiSelect"}
                }
            }
		]/**SCHEMA_DIFF*/,
		methods: {}
	};
});
