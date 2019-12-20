define("UsrTrainingSection", [], function() {
	return {
		entitySchemaName: "UsrTraining",
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				// Indicates that an operation of adding an item to the page is being executed.
				"operation": "insert",
				// Meta-name of the parent control item where the button is added.
				"parentName": "CombinedModeActionButtonsCardLeftContainer",
				// Indicates that the button is added to the control items collection
				// of the parent item (which name is specified in the parentName).
				"propertyName": "items",
				// Meta-name of the added button. .
				"name": "MainContactButton",
				// Supplementary properties of the item.
				"values": {
					// Type of the added item is button.
					itemType: Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.RED,
					// Binding the button title to a localizable string of the schema.
					caption: "MY BUTTON",
					// Binding the button press handler method.
					click: {bindTo: "onMyButtonClick"},
					// Binding the property of the button availability.
					//enabled: {bindTo: "isAccountPrimaryContactSet"},
					// Setting the field location.
					"layout": {
						"column": 1,
						"row": 6,
						"colSpan": 1
					}
				}
			}
		]/**SCHEMA_DIFF*/,
		methods: {}
	};
});
