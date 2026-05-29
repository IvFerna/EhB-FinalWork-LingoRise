const express = require("express");
const cors = require("cors");
const path = require("path");

const ttsRoutes = require("./routes/tts.routes");

const app = express();

app.use(cors());

app.use(express.json());

app.use("/storage", express.static(path.join(__dirname, "storage")));

app.use("/api/tts", ttsRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));

