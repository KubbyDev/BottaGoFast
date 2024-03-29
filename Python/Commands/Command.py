import Connection
from Commands import NetworkAnswer, PythonCode


# Pour ajouter une commande:
# - Creer un fichier ayant le nom de la commande et declarant une variable command_id = "NomDeLaCommande"
# - Declarer une fonction execute(parameters) qui execute la commande et renvoie l'eventuelle reponse (string)
# - Dans Command.py, dans execute, ajouter (sans oublier l'import):
#       if(firstarg == NomDeLaCommande.command_id):
#           return NomDeLaCommande.execute(rest)


# Recoit l'ordre et traite la presence d'id ou pas (requette ou ordre simple)
def receive(order):
    firstarg, rest = separate_first_arg(order)
    # Si le premier mot est un nombre alors on renvoie ce nombre en prefixe de la reponse (id de requette)
    if(firstarg.isdigit()):
        # Execute la commande sans le premier argument
        res = execute(rest)
        Connection.send_command(firstarg + " " + res if res is not None else "")
    else:
        # Execute la commande complete
        execute(order)


# Traite l'ordre
def execute(order):
    firstarg, rest = separate_first_arg(order)
    # Le premier argument est le type de commande
    if(firstarg == NetworkAnswer.command_id):
        return NetworkAnswer.execute(rest)
    if(firstarg == PythonCode.command_id):
        return PythonCode.execute(rest)
    # Si l'ordre est inconnu
    print("Couldn't find order " + firstarg)
    return None


# Renvoie un Tuple contenant (le premier argument, le reste)
def separate_first_arg(order):
    i = 0
    while(i < len(order) and order[i] != ' '):
        i += 1
    return (order[:i], order[i+1:])
