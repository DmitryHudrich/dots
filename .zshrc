# If you come from bash you might have to change your $PATH.
# export PATH=$HOME/bin:/usr/local/bin:$PATH

export ZSH="$HOME/.oh-my-zsh"

ZSH_THEME="avit"

plugins=( 
    git
    archlinux
    zsh-autosuggestions
    zsh-syntax-highlighting
    vi-mode
)

source $ZSH/oh-my-zsh.sh

# Check archlinux plugin commands here
# https://github.com/ohmyzsh/ohmyzsh/tree/master/plugins/archlinux


# Display Pokemon-colorscripts
# Project page: https://gitlab.com/phoneybadger/pokemon-colorscripts#on-other-distros-and-macos
pokemon-colorscripts --no-title -s -r

# Set-up FZF key bindings (CTRL R for fuzzy history finder)
source <(fzf --zsh)

eval "$(zoxide init zsh)"

# aliases

alias hx=helix
alias "docker compose"=docker-compose

alias gac="git add --all && git commit -m"
# Add .NET Core SDK tools
export PATH="$PATH:/home/w1thluv/.dotnet/tools"
