const mongoose = require("mongoose");

const settingSchema = new mongoose.Schema({
  userId: {
    type: mongoose.Schema.Types.ObjectId,
    required: true,
  },
  lightBackgroundColor: {
    type: String,
    required: true,
  },
  darkBackgroundColor: {
    type: String,
    required: true,
  },
  lightTextColor: {
    type: String,
    required: true,
  },
  darkTextColor: {
    type: String,
    required: true,
  },
  lastUpdate: {
    type: Date
  }
});

module.exports = mongoose.model("Setting", settingSchema);
