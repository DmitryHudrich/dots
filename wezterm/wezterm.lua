local wezterm = require("wezterm")
local config = wezterm.config_builder()

local tabs = require("config.tabs")
local keybindings = require("config.keybindings")
local merge_tables = require("config.utils").merge_tables

config.colors = {} --  NOTE: dont touch this, use utils.merge_tables
config.window_frame = {} --  NOTE: dont touch this, use utils.merge_tables

config.color_scheme = "Catppuccin Mocha"
config.font = wezterm.font("JetBrainsMono Nerd Font")
config.font_size = 12
config.check_for_updates = false
config.enable_wayland = false
config.enable_tab_bar = true
config.use_fancy_tab_bar = false
config.tab_bar_at_bottom = false
config.hide_tab_bar_if_only_one_tab = true
config.show_new_tab_button_in_tab_bar = false
config.window_close_confirmation = "NeverPrompt"
config.window_background_opacity = 0.8
config.text_background_opacity = 0.8
config.xcursor_theme = "Bibata-Modern-Ice"

config.window_padding = {
	left = 15,
	right = 15,
	top = 15,
	bottom = 15,
}

merge_tables(config.colors, {
	cursor_bg = "#dbdbdb",
	cursor_fg = "#151523",
	background = "#0F1012",
})

keybindings.apply(config)
tabs.apply(config)

return config
