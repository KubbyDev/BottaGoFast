import time


inPath = "F:/PY_orders.txt"
outPath = "F:/CS_orders.txt"
lastUpdateMoment = 0


# La fonction a appeller quand un ordre est recu (peut etre changee par set_order_event)
def order_event():
    pass


# Regarde si un ordre a ete recu
def look_for_order():
    try:
        # Lis le fichier
        file = open(inPath, "r")
        filecontent = file.read()
        file.close()
        # Vide le fichier si besoin
        if(filecontent != ""):
            open(inPath, "w").close()
    except PermissionError:
        # Si le fichier est en cours d'utilisation on abandonne
        return
    # Appelle la fonction d'evenement pour chaque ordre
    orders = filecontent.split('\n')
    for order in orders:
        if (order != ""):
            orderEvent(order)


# Definit la fonction a appeller quand un ordre est recu
def set_order_event(receiver):
    global orderEvent
    orderEvent = receiver


# Met a jour le systeme de detection d'ordres
def update():
    global lastUpdateMoment
    if(time.time() - lastUpdateMoment > 0.001):
        lastUpdateMoment = time.time()
        look_for_order()


# Envoie une commande. Attention, ne doit pas contenir de \n
def send_command(command):
    success = False
    while(not success):
        try:
            file = open(outPath, "a+")
            file.write(command + "\n")
            file.close()
            success = True
        except PermissionError:
            # Si le fichier est en cours d'utilisation on reessaye
            pass
