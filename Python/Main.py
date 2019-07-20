import Connection
import Commands.Command as Command

Connection.set_order_event(Command.receive)

while(True):
    Connection.update()
