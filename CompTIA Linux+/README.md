<!-- YAML Data from '.\CompTIA Linux+\Linux+.yaml' -->
<!-- At: 2/11/2024 1:47:51 PM -->

```yaml
###
# 90% of all "*** Facts" pages are useless or redundant, so they don't need to be reviewed all the time.
# It is recommended to at least skim them, but they should only really be read if highlighted in this document
#
# SKIP)     = Skip this lesson or section as it is not important to the student
# LOOK)     = Student should read what this page contains
# SECT)     = Specific boxes will be selected for viewing. They will be specified by "SKIP)" or "LOOK)"
# OPT)      = Optional; It's up to the student if they'd like to read it
# LATER)    = Useful, but not this early (Come back to it later)
# BOLD      = Important; Should be talked about with students
#
# N/A = No further information; Treat this module as usual (Just helps with formatting)
#
# At the end of the day, it's up to the student to determine if they want to skip something.
# This is just an easier way to learn the material.
#
# Tree view glossary model
#
#   X.X Module Name:
#       X.X Sub-Module Name:
#           X.X Assignment:
#               Information boxes (Found in "Facts" pages)
#
#   If the line does not end with a ":", then that is the end of the tree (There will be no further item
#   associated with it, based on YAML specification)
###

MARKDOWN) [TestOut Guides](https://guide.testout.com/)

MARKDOWN_L) [TestOut Linux+ Course Overview](https://guide.testout.com/overview/lxv6)

[
  Start with explaining how BASH parses command input

  For example;
    uname --help

  This means nothing to a student new to bash.
  You can tell them this gives info about the command, but then what?
  Is this special to the command? What signifies the command or how to pull up more help?
  Etc.

  Teach them how bash parses commands, so 'uname --help' becomes;
    'uname' [command]
    '--help' [arg0]

  Expand this to other built-in commands to show that this is a rule of standardization.
    'ls --help';
      'ls' [command]
      '--help' [arg0]

    'ls -la'
      'ls' [command]
      '-la' [arg1]

    [The same as]
      'ls' [command]
      '-l' [arg0]
      '-a' [arg1]


  Teach them to teach themselves.

  MARKDOWN_L) [Bash order of operations](https://mywiki.wooledge.org/BashParser)
  MARKDOWN_L) [Process Diagram](https://stuff.lhunath.com/parser.png)

  Bash order of operations;
    1. Read data to execute [Type in command]
    2. Process quotes [Evaluate quotation]
    3. Split the read data into commands [Split by ';' (separate commands on the same line)]
    4. Parse special operators [Piping, redirection, etc]
    4. Perform expansions [Variables, wildcards, etc]
    5. Split the command into a command name and arguments 
    6. Execute the command
]
```
